using System.Collections.Generic;
using DefaultNamespace.Data;
using DefaultNamespace.Models;
using DefaultNamespace.Presenters;
using DefaultNamespace.Views;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using R3;

namespace DefaultNamespace.Test
{
    public class MusicCardMVPTestController : MonoBehaviour
    {
        [Header("MVP Test Setup")]
        [SerializeField] private GameObject musicCardPrefab;
        [SerializeField] private Transform cardContainer;
        
        [Header("Test Data")]
        [SerializeField] private List<MusicCardData> testCards = new List<MusicCardData>();
        
        [Header("UI Controls")]
        [SerializeField] private Button nextCardButton;
        [SerializeField] private Button previousCardButton;
        [SerializeField] private Button stateToOnBoardButton;
        [SerializeField] private Button stateToReservedButton;
        [SerializeField] private Button stateToInDeckButton;
        [SerializeField] private Button stateToInResourcesButton;
        [SerializeField] private TextMeshProUGUI currentCardInfoText;
        [SerializeField] private TextMeshProUGUI modelStateText;
        
        [Header("Test Card Properties")]
        [SerializeField] private Sprite[] testCardImages;
        
        private List<MVPTestCard> mvpCards = new List<MVPTestCard>();
        private int currentCardIndex = 0;
        
        private class MVPTestCard
        {
            public MusicCardModel Model { get; }
            public MusicCardView View { get; }
            public MusicCardPresenter Presenter { get; }
            public GameObject GameObject { get; }
            
            public MVPTestCard(MusicCardModel model, MusicCardView view, MusicCardPresenter presenter, GameObject gameObject)
            {
                Model = model;
                View = view;
                Presenter = presenter;
                GameObject = gameObject;
            }
        }
        
        private void Start()
        {
            SetupButtons();
            LoadOrCreateTestCards();
            CreateMVPCards();
            ShowCurrentCard();
        }
        
        private void SetupButtons()
        {
            if (nextCardButton != null)
                nextCardButton.onClick.AddListener(ShowNextCard);
                
            if (previousCardButton != null)
                previousCardButton.onClick.AddListener(ShowPreviousCard);
                
            if (stateToOnBoardButton != null)
                stateToOnBoardButton.onClick.AddListener(() => ChangeCurrentCardState(MusicCardState.OnBoard));
                
            if (stateToReservedButton != null)
                stateToReservedButton.onClick.AddListener(() => ChangeCurrentCardState(MusicCardState.Reserved));
                
            if (stateToInDeckButton != null)
                stateToInDeckButton.onClick.AddListener(() => ChangeCurrentCardState(MusicCardState.InDeck));
                
            if (stateToInResourcesButton != null)
                stateToInResourcesButton.onClick.AddListener(() => ChangeCurrentCardState(MusicCardState.InPlayerResources));
        }
        
        private void LoadOrCreateTestCards()
        {
            // Jeśli mamy przypisane karty w edytorze, użyj ich
            if (testCards.Count > 0)
                return;
                
            // W przeciwnym razie stwórz testowe karty programowo
            CreateTestCardData();
        }
        
        private void CreateTestCardData()
        {
            // Stwórz różnorodne testowe karty
            for (int i = 0; i < 4; i++)
            {
                var cardData = ScriptableObject.CreateInstance<MusicCardData>();
                
                cardData.id = $"mvp_test_card_{i}";
                cardData.level = i + 1;
                cardData.points = (i + 1) * 2;
                cardData.cardName = $"MVP Test Card {i + 1}";
                cardData.cardDescription = $"Test card {i + 1} for demonstrating MVP pattern functionality.";
                
                // Przypisz różne typy zasobów
                var resourceTypes = new[] { ResourceType.Melody, ResourceType.Harmony, ResourceType.Rhythm, ResourceType.Instrumentation };
                cardData.resourceProvided = resourceTypes[i % resourceTypes.Length];
                
                // Ustaw różne koszty
                cardData.cost = new ResourceCost(
                    i == 0 ? 2 : 0,  // melody
                    i == 1 ? 2 : 0,  // harmony
                    i == 2 ? 2 : 0,  // rhythm
                    i == 3 ? 2 : 0,  // instrumentation
                    1                // dynamics (wszystkie mają 1)
                );
                
                // Przypisz obrazek jeśli dostępny
                if (testCardImages != null && testCardImages.Length > 0)
                {
                    cardData.cardImage = testCardImages[i % testCardImages.Length];
                }
                
                testCards.Add(cardData);
            }
        }
        
        private void CreateMVPCards()
        {
            // Wyczyść istniejące karty
            ClearMVPCards();
            
            foreach (var cardData in testCards)
            {
                // Utwórz GameObject karty
                var cardObject = Instantiate(musicCardPrefab, cardContainer);
                var cardView = cardObject.GetComponent<MusicCardView>();
                
                if (cardView == null)
                {
                    Debug.LogError("MusicCardView component not found on prefab!");
                    DestroyImmediate(cardObject);
                    continue;
                }
                
                // Utwórz Model
                var cardModel = new MusicCardModel(cardData);
                
                // Utwórz Presenter (łączy Model i View)
                var cardPresenter = new MusicCardPresenter(cardView, cardModel);
                
                // Skonfiguruj View z danymi
                cardView.Setup(cardData);
                
                // Dodaj do listy MVP kart
                var mvpCard = new MVPTestCard(cardModel, cardView, cardPresenter, cardObject);
                mvpCards.Add(mvpCard);
                
                // Domyślnie ukryj kartę
                cardObject.SetActive(false);
                
                // Nasłuchuj zmian stanu dla logowania
                cardModel.State.Subscribe(state => 
                {
                    Debug.Log($"Card '{cardData.cardName}' state changed to: {state}");
                    UpdateModelStateText();
                });
                
                
                cardModel.OwnerId.Subscribe(ownerId => 
                {
                    if (!string.IsNullOrEmpty(ownerId))
                        Debug.Log($"Card '{cardData.cardName}' owner changed to: {ownerId}");
                });
            }
        }
        
        private void ShowCurrentCard()
        {
            // Ukryj wszystkie karty
            foreach (var mvpCard in mvpCards)
            {
                mvpCard.GameObject.SetActive(false);
            }
            
            if (mvpCards.Count == 0)
                return;
                
            // Upewnij się, że indeks jest poprawny
            currentCardIndex = Mathf.Clamp(currentCardIndex, 0, mvpCards.Count - 1);
            
            // Pokaż aktualną kartę
            var currentCard = mvpCards[currentCardIndex];
            currentCard.GameObject.SetActive(true);
            
            // Aktualizuj UI
            UpdateCardInfoText();
            UpdateModelStateText();
        }
        
        private void ShowNextCard()
        {
            if (mvpCards.Count == 0) return;
            
            currentCardIndex = (currentCardIndex + 1) % mvpCards.Count;
            ShowCurrentCard();
        }
        
        private void ShowPreviousCard()
        {
            if (mvpCards.Count == 0) return;
            
            currentCardIndex = (currentCardIndex - 1 + mvpCards.Count) % mvpCards.Count;
            ShowCurrentCard();
        }
        
        private void ChangeCurrentCardState(MusicCardState newState)
        {
            if (mvpCards.Count == 0 || currentCardIndex >= mvpCards.Count) return;
            
            var currentCard = mvpCards[currentCardIndex];
            currentCard.Model.SetState(newState);
            
            Debug.Log($"Changed card state to: {newState}");
        }
        
        private void UpdateCardInfoText()
        {
            if (currentCardInfoText == null || mvpCards.Count == 0) return;
            
            var currentCard = mvpCards[currentCardIndex];
            var cardData = testCards[currentCardIndex];
            
            currentCardInfoText.text = $"Card {currentCardIndex + 1}/{mvpCards.Count}\n" +
                                     $"Name: {cardData.cardName}\n" +
                                     $"Level: {cardData.level} | Points: {cardData.points}\n" +
                                     $"Provides: {cardData.resourceProvided.GetDisplayName()}\n" +
                                     $"Total Cost: {cardData.cost.TotalCost()}";
        }
        
        private void UpdateModelStateText()
        {
            if (modelStateText == null || mvpCards.Count == 0) return;
            
            var currentCard = mvpCards[currentCardIndex];
            var model = currentCard.Model;
            
            modelStateText.text = $"MODEL STATE:\n" +
                                $"State: {model.State.CurrentValue}\n" +
                                $"Owner: {(string.IsNullOrEmpty(model.OwnerId.CurrentValue) ? "None" : model.OwnerId.CurrentValue)}\n" +
                                $"Board Position: {(model.BoardPosition.CurrentValue == -1 ? "N/A" : model.BoardPosition.CurrentValue.ToString())}";
        }
        
        private void ClearMVPCards()
        {
            foreach (var mvpCard in mvpCards)
            {
                mvpCard.Model?.Dispose();
                if (mvpCard.GameObject != null)
                    DestroyImmediate(mvpCard.GameObject);
            }
            mvpCards.Clear();
        }
        
        private void OnDestroy()
        {
            ClearMVPCards();
        }
        
        [ContextMenu("Test MVP Pattern")]
        public void TestMVPPattern()
        {
            Debug.Log("=== MVP Pattern Test ===");
            
            for (int i = 0; i < mvpCards.Count; i++)
            {
                var mvpCard = mvpCards[i];
                var cardData = testCards[i];
                
                Debug.Log($"Card {i + 1}: {cardData.cardName}");
                Debug.Log($"  Model State: {mvpCard.Model.State.CurrentValue}");
                Debug.Log($"  View Active: {mvpCard.View.gameObject.activeInHierarchy}");
                Debug.Log($"  Presenter Connected: {mvpCard.Presenter != null}");
            }
        }
        
        [ContextMenu("Test State Changes")]
        public void TestStateChanges()
        {
            if (mvpCards.Count == 0) return;
            
            var testCard = mvpCards[0];
            Debug.Log("Testing state changes on first card...");
            
            testCard.Model.SetState(MusicCardState.OnBoard);
            testCard.Model.SetState(MusicCardState.Reserved);
            testCard.Model.SetState(MusicCardState.InPlayerResources);
            testCard.Model.SetState(MusicCardState.InDeck);
        }
    }
} 