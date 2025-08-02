using System.Collections.Generic;
using BrunoMikoski.AnimationSequencer;
using Cysharp.Threading.Tasks;
using DefaultNamespace.Elements;
using R3;
using TMPro;
using UnityEngine;

namespace UI.CreatePlayerWindow
{
    public class CreatePlayerWindowView : MonoBehaviour
    {
        public Subject<string> OnAddPlayerButtonClicked { get; private set; } = new Subject<string>();
        public Subject<Unit> OnCloseButtonClicked { get; private set; } = new Subject<Unit>();

        [SerializeField] 
        private TMP_InputField playerNameInputField;

        [SerializeField]
        private ButtonElement addPlayerButton;

        [SerializeField]
        private ButtonElement closeButton;

        [SerializeField]
        private AnimationSequencerController openAnimation;

        [SerializeField]
        private AnimationSequencerController closeAnimation;


        public void Awake()
        {
            addPlayerButton.OnClick.Subscribe(_ => OnAddPlayerButtonClicked.OnNext(playerNameInputField.text)).AddTo(this);
            closeButton.OnClick.Subscribe(_ => OnCloseButtonClicked.OnNext(Unit.Default)).AddTo(this);
        }

        public async UniTask PlayOpenAnimation()
        {
            await openAnimation.PlayAsync();
        }

        public async UniTask PlayCloseAnimation()
        {
            playerNameInputField.text = "";
            await closeAnimation.PlayAsync();
        }
    }
}