using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace UI.MusicCardDetailsPanel {
    
    [System.Serializable]
    public class LevelStartPositions {
        [SerializeField] public RectTransform[] positions = new RectTransform[4];
        
        public RectTransform GetPosition(int position) {
            if (position < 0 || position >= positions.Length) {
                Debug.LogError($"Invalid position index: {position}. Must be between 0 and {positions.Length - 1}");
                return positions[0]; // fallback to first position
            }
            return positions[position];
        }
    }
    
    public class MusicCardDetailsPanelOpenAnimation : MonoBehaviour 
    {
        [Header("Animation Settings")]
        [SerializeField] private float cardMoveDuration = 0.8f;
        [SerializeField] private float backgroundFadeDuration = 0.5f;
        [SerializeField] private float cardScaleFrom = 1f;
        [SerializeField] private float cardScaleTo = 2f;
        [SerializeField] private float backgroundTargetAlpha = 0.8f;
        [SerializeField] private Ease cardMoveEase = Ease.OutCubic;
        [SerializeField] private Ease cardScaleEase = Ease.OutBack;
        [SerializeField] private Ease backgroundFadeEase = Ease.OutQuad;
        
        [Header("Positions Configuration")]
        [SerializeField] private LevelStartPositions[] levelStartPositions = new LevelStartPositions[3];
        [SerializeField] private RectTransform cardEndPosition;
        
        [Header("References")]
        [SerializeField] private DetailsMusicCardView detailsMusicCardView;
        [SerializeField] private Image backgroundImage;
        [SerializeField] private GameObject[] uiElementsToActivateAfterAnimation;
        
        private RectTransform cardRectTransform;
        private Vector3 originalCardPosition;
        private Vector3 originalCardScale;
        private Color originalBackgroundColor;
        
        private void Awake() {
            ValidateComponents();
            InitializeOriginalValues();
        }
        
        private void ValidateComponents() {
            if (detailsMusicCardView == null)
                Debug.LogError("DetailsMusicCardView is not assigned!", this);
            if (backgroundImage == null)
                Debug.LogError("BackgroundImage is not assigned!", this);
            if (cardEndPosition == null)
                Debug.LogError("CardEndPosition is not assigned!", this);
                
            // Validate level positions
            for (int level = 0; level < levelStartPositions.Length; level++) {
                if (levelStartPositions[level] == null) {
                    Debug.LogError($"LevelStartPositions for level {level} is not initialized!", this);
                    continue;
                }
                
                for (int pos = 0; pos < levelStartPositions[level].positions.Length; pos++) {
                    if (levelStartPositions[level].positions[pos] == null) {
                        Debug.LogError($"Position {pos} for level {level} is not assigned!", this);
                    }
                }
            }
        }
        
        private void InitializeOriginalValues() {
            if (detailsMusicCardView != null) {
                cardRectTransform = detailsMusicCardView.GetComponent<RectTransform>();
                if (cardRectTransform != null) {
                    originalCardPosition = cardRectTransform.position;
                    originalCardScale = cardRectTransform.localScale;
                }
            }
            
            if (backgroundImage != null) {
                originalBackgroundColor = backgroundImage.color;
            }
        }
        
        /// <summary>
        /// Odtwarza animację otwierania okna z określonym poziomem i pozycją
        /// </summary>
        /// <param name="level">Poziom (0-2)</param>
        /// <param name="position">Pozycja na poziomie (0-3)</param>
        public async UniTask PlayOpenAnimation(int level, int position) {
            if (!ValidateAnimationPrerequisites()) {
                return;
            }
            
            RectTransform startPosition = GetStartPosition(level, position);
            if (startPosition == null) {
                Debug.LogError($"Cannot get start position for level {level}, position {position}");
                return;
            }
            
            // Ustaw wartości początkowe
            SetInitialAnimationState(startPosition);
            
            // Utwórz sekwencję animacji
            var sequence = DOTween.Sequence();
            
            // Animacja tła (fade in)
            sequence.Append(backgroundImage.DOFade(backgroundTargetAlpha, backgroundFadeDuration)
                .SetEase(backgroundFadeEase));
            
            // Równoczesne animacje karty (pozycja i skala)
            sequence.Join(cardRectTransform.DOMove(cardEndPosition.position, cardMoveDuration)
                .SetEase(cardMoveEase));
            
            sequence.Join(cardRectTransform.DOScale(cardScaleTo, cardMoveDuration)
                .SetEase(cardScaleEase));
            
            // Poczekaj na zakończenie animacji karty i tła
            await sequence.AsyncWaitForCompletion();
            
            // Włącz pozostałe elementy UI
            ActivateUIElements();
        }
        
        /// <summary>
        /// Odtwarza animację z domyślnymi parametrami (level 0, position 0)
        /// </summary>
        public async UniTask PlayOpenAnimation() {
            await PlayOpenAnimation(0, 0);
        }
        
        /// <summary>
        /// Ustawia wszystkie elementy na wartości końcowe (fallback gdy animacja się zepsuje)
        /// </summary>
        public void SetToFinalState() {
            if (!ValidateAnimationPrerequisites()) {
                return;
            }
            
            // Zatrzymaj wszystkie animacje
            DOTween.Kill(cardRectTransform);
            DOTween.Kill(backgroundImage);
            
            // Ustaw wartości końcowe
            cardRectTransform.position = cardEndPosition.position;
            cardRectTransform.localScale = Vector3.one * cardScaleTo;
            
            Color finalBackgroundColor = originalBackgroundColor;
            finalBackgroundColor.a = backgroundTargetAlpha;
            backgroundImage.color = finalBackgroundColor;
            
            // Włącz wszystkie elementy UI
            ActivateUIElements();
        }
        
        /// <summary>
        /// Resetuje panel do stanu początkowego z określonym poziomem i pozycją
        /// </summary>
        /// <param name="level">Poziom (0-2)</param>
        /// <param name="position">Pozycja na poziomie (0-3)</param>
        public void ResetToInitialState(int level, int position) {
            if (!ValidateAnimationPrerequisites()) {
                return;
            }
            
            // Zatrzymaj wszystkie animacje
            DOTween.Kill(cardRectTransform);
            DOTween.Kill(backgroundImage);
            
            RectTransform startPosition = GetStartPosition(level, position);
            if (startPosition != null) {
                cardRectTransform.position = startPosition.position;
            } else {
                cardRectTransform.position = originalCardPosition;
            }
            
            cardRectTransform.localScale = Vector3.one * cardScaleFrom;
            
            Color initialBackgroundColor = originalBackgroundColor;
            initialBackgroundColor.a = 0f;
            backgroundImage.color = initialBackgroundColor;
            
            // Wyłącz elementy UI
            DeactivateUIElements();
        }
        
        /// <summary>
        /// Resetuje panel do stanu początkowego z domyślnymi parametrami
        /// </summary>
        public void ResetToInitialState() {
            ResetToInitialState(0, 0);
        }
        
        private RectTransform GetStartPosition(int level, int position) {
            if (level < 0 || level >= levelStartPositions.Length) {
                Debug.LogError($"Invalid level: {level}. Must be between 0 and {levelStartPositions.Length - 1}");
                return null;
            }
            
            if (levelStartPositions[level] == null) {
                Debug.LogError($"LevelStartPositions for level {level} is null");
                return null;
            }
            
            return levelStartPositions[level].GetPosition(position);
        }
        
        private bool ValidateAnimationPrerequisites() {
            return cardRectTransform != null && 
                   backgroundImage != null && 
                   cardEndPosition != null;
        }
        
        private void SetInitialAnimationState(RectTransform startPosition) {
            // Ustaw pozycję początkową karty
            if (startPosition != null) {
                cardRectTransform.position = startPosition.position;
            }
            
            // Ustaw skalę początkową karty
            cardRectTransform.localScale = Vector3.one * cardScaleFrom;
            
            // Ustaw alpha tła na 0
            Color backgroundColor = originalBackgroundColor;
            backgroundColor.a = 0f;
            backgroundImage.color = backgroundColor;
            
            // Wyłącz elementy UI
            DeactivateUIElements();
        }
        
        private void ActivateUIElements() {
            if (uiElementsToActivateAfterAnimation == null) return;
            
            foreach (var element in uiElementsToActivateAfterAnimation) {
                if (element != null) {
                    element.SetActive(true);
                }
            }
        }
        
        private void DeactivateUIElements() {
            if (uiElementsToActivateAfterAnimation == null) return;
            
            foreach (var element in uiElementsToActivateAfterAnimation) {
                if (element != null) {
                    element.SetActive(false);
                }
            }
        }
    }
}