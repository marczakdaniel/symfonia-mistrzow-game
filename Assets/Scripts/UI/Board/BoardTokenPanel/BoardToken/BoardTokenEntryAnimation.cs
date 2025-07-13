using UnityEngine;
using Cysharp.Threading.Tasks;
using DG.Tweening;

namespace UI.Board.BoardTokenPanel.BoardToken
{
    public class BoardTokenEntryAnimation : MonoBehaviour
    {
        [SerializeField] private GameObject panel;
        [SerializeField] private GameObject token;
        
        [Header("Animation Settings")]
        [SerializeField] private float panelSlideDuration = 0.5f;
        [SerializeField] private float tokenJumpDuration = 0.3f;
        [SerializeField] private float tokenJumpDelay = 0.2f;
        [SerializeField] private float tokenJumpHeight = 50f;
        [SerializeField] private float tokenJumpDistance = 30f;
        [SerializeField] private float panelSlideDistance = 1100f;

        private Vector3 panelInitialPosition;
        private Vector3 tokenInitialPosition;

        private void Awake()
        {
            // Store initial positions
            if (panel != null)
                panelInitialPosition = panel.transform.localPosition;
            if (token != null)
                tokenInitialPosition = token.transform.localPosition;
        }

        public async UniTask Play()
        {
            // Reset positions to initial state
            ResetPositions();
            
            // Step 1: Panel slides up from bottom
            await SlidePanelUp();
            
            // Step 2: Token jumps to the left
            await JumpTokenLeft();
        }

        private void ResetPositions()
        {
            if (panel != null)
            {
                // Set panel below the screen
                Vector3 startPos = panelInitialPosition;
                startPos.y -= panelSlideDistance; // Move panel down
                panel.transform.localPosition = startPos;
            }
            
            if (token != null)
            {
                // Reset token to initial position
                token.transform.localPosition = tokenInitialPosition;
            }
        }

        private async UniTask SlidePanelUp()
        {
            if (panel == null) return;
            
            var tween = panel.transform.DOLocalMove(panelInitialPosition, panelSlideDuration)
                .SetEase(Ease.OutBack);
            
            await UniTask.WaitWhile(() => tween.IsActive());
        }

        private async UniTask JumpTokenLeft()
        {
            if (token == null) return;
            
            // Wait for the specified delay
            await UniTask.Delay((int)(tokenJumpDelay * 1000));
            
            // Calculate jump target position (left side)
            Vector3 jumpTarget = tokenInitialPosition;
            jumpTarget.x -= tokenJumpDistance;
            
            // Create jump animation sequence
            var jumpSequence = DOTween.Sequence();
            
            // First jump up and left
            jumpSequence.Append(token.transform.DOLocalJump(jumpTarget, tokenJumpHeight, 1, tokenJumpDuration * 0.5f)
                .SetEase(Ease.OutQuad));

            
            await UniTask.WaitWhile(() => jumpSequence.IsActive());
        }

        public void ResetAnimation()
        {
            if (panel != null)
                panel.transform.localPosition = panelInitialPosition;
            if (token != null)
                token.transform.localPosition = tokenInitialPosition;
        }
    }
}