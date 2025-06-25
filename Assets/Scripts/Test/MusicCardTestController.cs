using System.Collections.Generic;
using DefaultNamespace.Data;
using DefaultNamespace.Views;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace.Test
{
    public class MusicCardTestController : MonoBehaviour
    {
        [Header("Prefab References")]
        [SerializeField] private GameObject musicCardPrefab;
        
        [Header("Test Data")]
        [SerializeField] private List<MusicCardData> testCards = new List<MusicCardData>();
        
        [Header("UI Elements")]
        [SerializeField] private Transform cardContainer;
        [SerializeField] private Button nextCardButton;
        [SerializeField] private Button previousCardButton;
        [SerializeField] private Button createRandomCardButton;
        [SerializeField] private TextMeshProUGUI currentCardIndexText;
        
        [Header("Test Card Properties")]
        [SerializeField] private Sprite[] testCardImages;
        
        private List<GameObject> instantiatedCards = new List<GameObject>();
        private int currentCardIndex = 0;
        
        private void Start()
        {
            SetupButtons();
            CreateTestCards();
            ShowCurrentCard();
        }
        
        private void SetupButtons()
        {
            if (nextCardButton != null)
                nextCardButton.onClick.AddListener(ShowNextCard);
                
            if (previousCardButton != null)
                previousCardButton.onClick.AddListener(ShowPreviousCard);
                
            if (createRandomCardButton != null)
                createRandomCardButton.onClick.AddListener(CreateRandomCard);
        }
        
        private void CreateTestCards()
        {
            // Sprawdź czy już mamy testowe karty
            if (testCards.Count > 0)
                return;
                
            // Utwórz kilka testowych kart
            for (int i = 0; i < 5; i++)
            {
                var testCard = CreateTestCardData(i);
                testCards.Add(testCard);
            }
        }
        
        private MusicCardData CreateTestCardData(int index)
        {
            var cardData = ScriptableObject.CreateInstance<MusicCardData>();
            
            // Podstawowe właściwości karty
            cardData.id = $"test_card_{index}";
            cardData.level = Random.Range(1, 5);
            cardData.points = Random.Range(1, 10);
            cardData.cardName = $"Test Card {index + 1}";
            cardData.cardDescription = $"This is test card number {index + 1} for testing purposes.";
            
            // Typ zasobu dostarczanego przez kartę
            var resourceTypes = System.Enum.GetValues(typeof(ResourceType));
            cardData.resourceProvided = (ResourceType)resourceTypes.GetValue(Random.Range(0, resourceTypes.Length));
            
            // Koszt karty
            cardData.cost = new ResourceCost(
                Random.Range(0, 3),  // melody
                Random.Range(0, 3),  // harmony
                Random.Range(0, 3),  // rhythm
                Random.Range(0, 3),  // instrumentation
                Random.Range(0, 3)   // dynamics
            );
            
            // Obrazek karty
            if (testCardImages != null && testCardImages.Length > 0)
            {
                cardData.cardImage = testCardImages[Random.Range(0, testCardImages.Length)];
            }
            
            return cardData;
        }
        
        private void CreateRandomCard()
        {
            var randomCard = CreateTestCardData(testCards.Count);
            testCards.Add(randomCard);
            
            // Przejdź do nowej karty
            currentCardIndex = testCards.Count - 1;
            ShowCurrentCard();
        }
        
        private void ShowCurrentCard()
        {
            // Usuń wszystkie istniejące karty
            ClearInstantiatedCards();
            
            if (testCards.Count == 0)
                return;
                
            // Upewnij się, że indeks jest w poprawnym zakresie
            currentCardIndex = Mathf.Clamp(currentCardIndex, 0, testCards.Count - 1);
            
            // Utwórz nową kartę
            if (musicCardPrefab != null && cardContainer != null)
            {
                var cardObject = Instantiate(musicCardPrefab, cardContainer);
                var cardView = cardObject.GetComponent<MusicCardView>();
                
                if (cardView != null)
                {
                    cardView.Setup(testCards[currentCardIndex]);
                    instantiatedCards.Add(cardObject);
                }
            }
            
            // Aktualizuj tekst indeksu
            UpdateCardIndexText();
        }
        
        private void ShowNextCard()
        {
            if (testCards.Count == 0)
                return;
                
            currentCardIndex = (currentCardIndex + 1) % testCards.Count;
            ShowCurrentCard();
        }
        
        private void ShowPreviousCard()
        {
            if (testCards.Count == 0)
                return;
                
            currentCardIndex = (currentCardIndex - 1 + testCards.Count) % testCards.Count;
            ShowCurrentCard();
        }
        
        private void ClearInstantiatedCards()
        {
            foreach (var card in instantiatedCards)
            {
                if (card != null)
                    DestroyImmediate(card);
            }
            instantiatedCards.Clear();
        }
        
        private void UpdateCardIndexText()
        {
            if (currentCardIndexText != null)
            {
                currentCardIndexText.text = $"Card {currentCardIndex + 1} of {testCards.Count}";
            }
        }
        
        [ContextMenu("Test All Cards")]
        public void TestAllCards()
        {
            Debug.Log("Testing all cards:");
            for (int i = 0; i < testCards.Count; i++)
            {
                var card = testCards[i];
                Debug.Log($"Card {i}: {card.cardName} - Level: {card.level}, Points: {card.points}, " +
                         $"Provides: {card.resourceProvided}, Total Cost: {card.cost.TotalCost()}");
            }
        }
        
        [ContextMenu("Clear All Test Cards")]
        public void ClearAllTestCards()
        {
            testCards.Clear();
            ClearInstantiatedCards();
            currentCardIndex = 0;
            UpdateCardIndexText();
        }
    }
} 