using System.Collections.Generic;
using BrunoMikoski.AnimationSequencer;
using Cysharp.Threading.Tasks;
using DefaultNamespace.Elements;
using R3;
using UnityEngine;

namespace UI.CreateGameWindow
{
    public class CreateGameWindowView : MonoBehaviour
    {
        public Subject<Unit> OnAddPlayerButtonClicked { get; private set; } = new Subject<Unit>();
        public Subject<Unit> OnStartGameButtonClicked { get; private set; } = new Subject<Unit>();
        public Subject<Unit> OnCloseButtonClicked { get; private set; } = new Subject<Unit>();

        [SerializeField] 
        private ButtonElement addPlayerButton;

        [SerializeField]
        private ButtonElement startGameButton;

        [SerializeField]
        private ButtonElement closeButton;

        [SerializeField]
        private AnimationSequencerController openAnimation;

        [SerializeField]
        private AnimationSequencerController closeAnimation;

        [SerializeField]
        private SinglePlayerElement[] singlePlayerElements;

        public void Awake()
        {
            addPlayerButton.OnClick.Subscribe(_ => OnAddPlayerButtonClicked.OnNext(Unit.Default)).AddTo(this);
            startGameButton.OnClick.Subscribe(_ => OnStartGameButtonClicked.OnNext(Unit.Default)).AddTo(this);
            closeButton.OnClick.Subscribe(_ => OnCloseButtonClicked.OnNext(Unit.Default)).AddTo(this);
        }

        public void SetPlayers(List<string> playerNames)
        {
            for (int i = 0; i < playerNames.Count; i++)
            {
                singlePlayerElements[i].Initialize(playerNames[i]);
                singlePlayerElements[i].SetActive(true);
            }

            for (int i = playerNames.Count; i < singlePlayerElements.Length; i++)
            {
                singlePlayerElements[i].SetActive(false);
            }
        }

        public async UniTask PlayOpenAnimation()
        {
            SetPlayers(new List<string>());
            await openAnimation.PlayAsync();
        }

        public async UniTask PlayCloseAnimation()
        {
            await closeAnimation.PlayAsync();
        }
    }

}