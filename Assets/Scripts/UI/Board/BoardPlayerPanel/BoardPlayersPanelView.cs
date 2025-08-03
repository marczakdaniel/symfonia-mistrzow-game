using BrunoMikoski.AnimationSequencer;
using Cysharp.Threading.Tasks;
using DefaultNamespace.Elements;
using R3;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
        private GameObject activePlayerIndicator;

        [SerializeField]
        private TextMeshProUGUI playerPointsText;
        
        [SerializeField]
        private AnimationSequencerController activateAnimation;

        [SerializeField]
        private AnimationSequencerController currentPlayerAnimation;

        [SerializeField]
        private AnimationSequencerController stopCurrentPlayerAnimation;

        public void SetPlayerImage(Sprite sprite)
        {
            playerImage.sprite = sprite;
        }

        public void SetActivePlayerIndicator(bool isActive)
        {
            activePlayerIndicator.SetActive(isActive);
        }

        public void SetPlayerPoints(int points)
        {
            playerPointsText.text = points.ToString();
        }

        public async UniTask PlayActivateAnimation()
        {
            await activateAnimation.PlayAsync();
        }

        public async UniTask PlayCurrentPlayerAnimation()
        {
            await currentPlayerAnimation.PlayAsync();
        }

        public async UniTask PlayStopCurrentPlayerAnimation()
        {
            await stopCurrentPlayerAnimation.PlayAsync();
        }

        public void Awake()
        {
            buttonElement.OnClick.Subscribe(OnClick.OnNext).AddTo(this);
        }
    }
}