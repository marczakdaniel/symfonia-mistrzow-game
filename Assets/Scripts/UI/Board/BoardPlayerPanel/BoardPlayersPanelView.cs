using BrunoMikoski.AnimationSequencer;
using Cysharp.Threading.Tasks;
using DefaultNamespace.Elements;
using R3;
using UI.Board.BoardPlayerPanel.BoardPlayerPanelSingleResource;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Coffee.UIEffects;

namespace UI.Board.BoardPlayerPanel
{
    public class BoardPlayerPanelView : MonoBehaviour
    {
        public Subject<Unit> OnClick { get; private set; } = new Subject<Unit>();

        [SerializeField]
        private ButtonElement buttonElement;

        [SerializeField] 
        private Image playerImage;

        [SerializeField]
        private TextMeshProUGUI playerPointsText;

        [SerializeField]
        private AnimationSequencerController pointsAnimation;
        
        [SerializeField]
        private AnimationSequencerController activateAnimation;

        [SerializeField]
        private UIEffect currentPlayerEffect;
        
        [SerializeField]
        private BoardPlayerPanelSingleResourceView[] singleResourceViews = new BoardPlayerPanelSingleResourceView[6];

        public BoardPlayerPanelSingleResourceView[] SingleResourceViews => singleResourceViews;

        public void SetPlayerImage(Sprite sprite)
        {
            playerImage.sprite = sprite;
        }

        public void SetPlayerPoints(int points)
        {
            playerPointsText.text = points.ToString();
            pointsAnimation.ClearPlayingSequence();
            pointsAnimation.ResetToInitialState();
            pointsAnimation.Play();
        }

        public async UniTask PlayActivateAnimation()
        {
            await activateAnimation.PlayAsync();
        }

        public async UniTask PlayCurrentPlayerAnimation()
        {
            currentPlayerEffect.enabled = true;
            await UniTask.CompletedTask;
        }

        public async UniTask PlayStopCurrentPlayerAnimation()
        {
            currentPlayerEffect.enabled = false;
            await UniTask.CompletedTask;
        }

        public void Awake()
        {
            buttonElement.OnClick.Subscribe(OnClick.OnNext).AddTo(this);
        }
    }
}