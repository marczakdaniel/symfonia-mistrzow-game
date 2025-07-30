using DefaultNamespace.Elements;
using R3;
using UnityEngine;
using TMPro;
using Cysharp.Threading.Tasks;
using BrunoMikoski.AnimationSequencer;

namespace UI.StartTurnWindow
{
    public class StartTurnWindowView : MonoBehaviour
    {
        public Subject<Unit> OnStartTurnButtonClick { get; private set; } = new Subject<Unit>();

        [SerializeField] private ButtonElement startTurnButton;
        [SerializeField] private TextMeshProUGUI currentPlayerNameText;

        [SerializeField] private AnimationSequencerController openAnimationSequencerController;
        [SerializeField] private AnimationSequencerController closeAnimationSequencerController;

        public void Awake()
        {
            startTurnButton.OnClick.Subscribe(OnStartTurnButtonClick.OnNext);
        }

        public async UniTask OpenWindow()
        {
            await openAnimationSequencerController.PlayAsync();
        }

        public async UniTask CloseWindow()
        {
            UnityEngine.Debug.Log("[StartTurnWindowView] CloseWindow");
            await closeAnimationSequencerController.PlayAsync();
        }

        public void SetCurrentPlayerName(string name)
        {
            currentPlayerNameText.SetText(name);
        }
    }
}