using System.Collections.Generic;
using Assets.Scripts.Data;
using Assets.Scripts.UI.Elements;
using BrunoMikoski.AnimationSequencer;
using Cysharp.Threading.Tasks;
using DefaultNamespace.Elements;
using Models;
using R3;
using UnityEngine;

namespace Assets.Scripts.UI.ConcertCardsWindow
{
    public class ConcertCardsWindowView : MonoBehaviour
    {
        public Subject<Unit> OnCloseButtonClicked { get; private set; } = new Subject<Unit>();

        [SerializeField]
        private ConcertCardElement[] concertCards = new ConcertCardElement[5];

        [SerializeField]
        private ButtonElement closeButton;

        [SerializeField]
        private AnimationSequencerController openAnimation;

        [SerializeField]
        private AnimationSequencerController closeAnimation;

        public void Initialize(List<ConcertCardData> data, List<ConcertCardState> cardStates, List<Sprite> ownerAvatars)
        {
            for (int i = 0; i < data.Count; i++)
            {
                if (i >= concertCards.Length)
                {
                    break;
                }


                concertCards[i].Initialize(data[i], cardStates[i], ownerAvatars[i]);
                concertCards[i].gameObject.SetActive(true);
            }

            for (int i = data.Count; i < concertCards.Length; i++)
            {
                concertCards[i].gameObject.SetActive(false);
            }

        }

        public async UniTask PlayClaimAnimation()
        {
            var tasks = new List<UniTask>();
            foreach (var card in concertCards)
            {
                tasks.Add(card.PlayClaimAnimation());
            }

            await UniTask.WhenAll(tasks);
        }

        public async UniTask PlayOpenAnimation()
        {
            await openAnimation.PlayAsync();
        }

        public async UniTask PlayCloseAnimation()
        {
            await closeAnimation.PlayAsync();
        }

        public void Awake()
        {
            closeButton.OnClick.Subscribe(_ => OnCloseButtonClicked.OnNext(Unit.Default)).AddTo(this);
        }
    }
}