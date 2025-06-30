using ObservableCollections;
using System.Collections.Generic;
using System;
using System.Linq;
using R3;

namespace DefaultNamespace.Models
{
    public class PlayerMusicCardsModel : IDisposable
    {
        private string playerId;
        private ObservableList<MusicCardModel> playerResourcesCards = new ObservableList<MusicCardModel>();
        private ObservableList<MusicCardModel> playerReservedCards = new ObservableList<MusicCardModel>();
        private IEnumerable<MusicCardModel> allCards;
        private IDisposable disposable;

        public PlayerMusicCardsModel(string playerId, IEnumerable<MusicCardModel> allCards)
        {
            this.playerId = playerId;
            this.allCards = allCards ?? throw new ArgumentNullException(nameof(allCards));

            InitializeCards();
            SubscribeToCardStateChanges();
        }

        private void InitializeCards()
        {
            var initialCards = allCards.Where(card => card.State.CurrentValue == MusicCardState.InPlayerResources && card.OwnerId.CurrentValue == playerId).ToList();
            var initialReservedCards = allCards.Where(card => card.State.CurrentValue == MusicCardState.Reserved && card.OwnerId.CurrentValue == playerId).ToList();
            playerResourcesCards.AddRange(initialCards);
            playerReservedCards.AddRange(initialReservedCards);
        }

        private void SubscribeToCardStateChanges()
        {
            var d = Disposable.CreateBuilder();

            foreach (var card in allCards)
            {
                card.State.Subscribe(state => HandleCardStateChange(card, state)).AddTo(ref d);
            }

            disposable = d.Build();
        }   

        private void HandleCardStateChange(MusicCardModel card, MusicCardState state)
        {
            var isInPlayerResources = playerResourcesCards.Contains(card);
            var isInPlayerReserved = playerReservedCards.Contains(card);

            var shouldBeInPlayerResources = state == MusicCardState.InPlayerResources;
            var shouldBeInPlayerReserved = state == MusicCardState.Reserved;

            if (shouldBeInPlayerResources && !isInPlayerResources)
            {
                playerResourcesCards.Add(card);
            }
            else if (!shouldBeInPlayerResources && isInPlayerResources)
            {
                playerResourcesCards.Remove(card);
            }

            if (shouldBeInPlayerReserved && !isInPlayerReserved)
            {
                playerReservedCards.Add(card);
            }
            else if (!shouldBeInPlayerReserved && isInPlayerReserved)
            {
                playerReservedCards.Remove(card);
            }
        }

        public void Dispose()
        {
            disposable?.Dispose();
        }
    }
}