using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace UI.MusicCardDetailsPanel {
    public class MusicCardDetailsPanelCloseAnimation : MonoBehaviour 
    {
        [Header("Animation Settings")]
        [SerializeField] private float cardMoveDuration = 0.6f;
        [SerializeField] private float backgroundFadeDuration = 0.4f;
        [SerializeField] private float cardScaleFrom = 2f;
        [SerializeField] private float cardScaleTo = 1f;
        [SerializeField] private float backgroundStartAlpha = 0.8f;
        [SerializeField] private Ease cardMoveEase = Ease.InCubic;
        [SerializeField] private Ease cardScaleEase = Ease.InBack;
        [SerializeField] private Ease backgroundFadeEase = Ease.InQuad;
        
        [Header("Positions Configuration")]
        [SerializeField] private LevelStartPositions[] levelStartPositions = new LevelStartPositions[3];
        [SerializeField] private RectTransform cardEndPosition;
        
        [Header("References")]
        [SerializeField] private DetailsMusicCardView detailsMusicCardView;
        [SerializeField] private Image backgroundImage;
        [SerializeField] private GameObject[] uiElementsToDeactivateBeforeAnimation;
        
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
        /// Odtwarza animację zamykania okna z określonym poziomem i pozycją
        /// </summary>
        /// <param name="level">Poziom (0-2)</param>
        /// <param name="position">Pozycja na poziomie (0-3)</param>
        public async UniTask PlayCloseAnimation(int level, int position) {
            if (!ValidateAnimationPrerequisites()) {
                return;
            }
            
            RectTransform targetPosition = GetTargetPosition(level, position);
            if (targetPosition == null) {
                Debug.LogError($"Cannot get target position for level {level}, position {position}");
                return;
            }
            
            // Wyłącz elementy UI na początku animacji
            DeactivateUIElements();
            
            // Ustaw wartości początkowe animacji zamykania
            SetInitialCloseAnimationState();
            
            // Utwórz sekwencję animacji
            var sequence = DOTween.Sequence();
            
            // Równoczesne animacje karty (pozycja i skala)
            sequence.Append(cardRectTransform.DOMove(targetPosition.position, cardMoveDuration)
                .SetEase(cardMoveEase));
            
            sequence.Join(cardRectTransform.DOScale(cardScaleTo, cardMoveDuration)
                .SetEase(cardScaleEase));
            
            // Animacja tła (fade out) - zaczyna się nieco później
            sequence.Insert(cardMoveDuration * 0.3f, backgroundImage.DOFade(0f, backgroundFadeDuration)
                .SetEase(backgroundFadeEase));
            
            // Poczekaj na zakończenie wszystkich animacji
            await sequence.AsyncWaitForCompletion();
        }
        
        /// <summary>
        /// Odtwarza animację z domyślnymi parametrami (level 0, position 0)
        /// </summary>
        public async UniTask PlayCloseAnimation() {
            await PlayCloseAnimation(0, 0);
        }
        
        /// <summary>
        /// Ustawia wszystkie elementy na wartości końcowe animacji zamykania (fallback gdy animacja się zepsuje)
        /// </summary>
        /// <param name="level">Poziom (0-2)</param>
        /// <param name="position">Pozycja na poziomie (0-3)</param>
        public void SetToFinalCloseState(int level, int position) {
            if (!ValidateAnimationPrerequisites()) {
                return;
            }
            
            // Zatrzymaj wszystkie animacje
            DOTween.Kill(cardRectTransform);
            DOTween.Kill(backgroundImage);
            
            RectTransform targetPosition = GetTargetPosition(level, position);
            if (targetPosition != null) {
                cardRectTransform.position = targetPosition.position;
            } else {
                cardRectTransform.position = originalCardPosition;
            }
            
            cardRectTransform.localScale = Vector3.one * cardScaleTo;
            
            Color finalBackgroundColor = originalBackgroundColor;
            finalBackgroundColor.a = 0f;
            backgroundImage.color = finalBackgroundColor;
            
            // Wyłącz wszystkie elementy UI
            DeactivateUIElements();
        }
        
        /// <summary>
        /// Ustawia wszystkie elementy na wartości końcowe z domyślnymi parametrami
        /// </summary>
        public void SetToFinalCloseState() {
            SetToFinalCloseState(0, 0);
        }
        
        /// <summary>
        /// Resetuje panel do stanu otwartego (przydatne do debugowania)
        /// </summary>
        public void ResetToOpenState() {
            if (!ValidateAnimationPrerequisites()) {
                return;
            }
            
            // Zatrzymaj wszystkie animacje
            DOTween.Kill(cardRectTransform);
            DOTween.Kill(backgroundImage);
            
            // Ustaw wartości stanu otwartego
            cardRectTransform.position = cardEndPosition.position;
            cardRectTransform.localScale = Vector3.one * cardScaleFrom;
            
            Color openBackgroundColor = originalBackgroundColor;
            openBackgroundColor.a = backgroundStartAlpha;
            backgroundImage.color = openBackgroundColor;
            
            // Włącz elementy UI
            ActivateUIElements();
        }
        
        private RectTransform GetTargetPosition(int level, int position) {
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
        
        private void SetInitialCloseAnimationState() {
            // Ustaw pozycję końcową jako punkt wyjścia
            cardRectTransform.position = cardEndPosition.position;
            
            // Ustaw skalę początkową animacji zamykania
            cardRectTransform.localScale = Vector3.one * cardScaleFrom;
            
            // Ustaw alpha tła na wartość początkową
            Color backgroundColor = originalBackgroundColor;
            backgroundColor.a = backgroundStartAlpha;
            backgroundImage.color = backgroundColor;
        }
        
        private void ActivateUIElements() {
            if (uiElementsToDeactivateBeforeAnimation == null) return;
            
            foreach (var element in uiElementsToDeactivateBeforeAnimation) {
                if (element != null) {
                    element.SetActive(true);
                }
            }
        }
        
        private void DeactivateUIElements() {
            if (uiElementsToDeactivateBeforeAnimation == null) return;
            
            foreach (var element in uiElementsToDeactivateBeforeAnimation) {
                if (element != null) {
                    element.SetActive(false);
                }
            }
        }
    }
} 