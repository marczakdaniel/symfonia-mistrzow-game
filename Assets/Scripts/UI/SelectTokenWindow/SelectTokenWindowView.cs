using Cysharp.Threading.Tasks;
using UnityEngine;
using R3;
using DefaultNamespace.Elements;
using UI.SelectTokenWindow.SelectBoardToken;
using UI.SelectTokenWindow.ChoosenBoardTokenPanel;
using Assets.Scripts.UI.Elements;
using System.Collections.Generic;
using DefaultNamespace.Data;
using System;
using BrunoMikoski.AnimationSequencer;

namespace UI.SelectTokenWindow
{
    public class SelectTokenWindowView : MonoBehaviour
    {
        public Subject<Unit> OnCloseButtonClicked { get; private set; } = new Subject<Unit>();
        public Subject<Unit> OnAcceptButtonClicked { get; private set; } = new Subject<Unit>();
        
        public ChoosenBoardTokenPanelView ChoosenBoardTokenPanelView => choosenBoardTokenPanelView;
        public UniversalTokenElement[] SelectBoardTokens => selectBoardTokens;
        [SerializeField] private ChoosenBoardTokenPanelView choosenBoardTokenPanelView;
        [SerializeField] private ButtonElement closeButton;
        [SerializeField] private ButtonElement acceptButton;
        [SerializeField] private UniversalPlayerResourceElement[] playerTokens = new UniversalPlayerResourceElement[6];
        [SerializeField] private UniversalTokenElement[] selectBoardTokens = new UniversalTokenElement[5];

        [SerializeField] private SelectTokenWindowOpenAnimation openAnimation;
        [SerializeField] private AnimationSequencerController closeAnimation;
        [SerializeField] private AnimationSequencerController hideAnimation;

        private void Awake()
        {
            closeButton.OnClick.Subscribe(_ => OnCloseButtonClicked.OnNext(Unit.Default)).AddTo(this);
            acceptButton.OnClick.Subscribe(_ => OnAcceptButtonClicked.OnNext(Unit.Default)).AddTo(this);
        }

        public async UniTask OnCloseWindow()
        {
            await closeAnimation.PlayAsync();
            PlayHideWithDelay().Forget();
        }

        public async UniTask PlayHideWithDelay()
        {
            await UniTask.Delay(50);
            await hideAnimation.PlayAsync();
        }

        public async UniTask OnOpenWindow()
        {
            await openAnimation.PlayOpenAnimation();
        }

        public void InitializePlayerTokens(Dictionary<ResourceType, int> tokens, Dictionary<ResourceType, int> currentPlayerTokens)
        {
            for (int i = 0; i < playerTokens.Length; i++)
            {
                playerTokens[i].Initialize((ResourceType)i, tokens[(ResourceType)i], currentPlayerTokens[(ResourceType)i]);
            }
        }
    }
}