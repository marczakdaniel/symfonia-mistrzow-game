using ObservableCollections;
using DefaultNamespace.Models;
using System.Collections.Generic;
using System;
using System.Linq;
using R3;
using UnityEngine.AI;
using System.Collections.Specialized;

namespace DefaultNamespace.Models
{
    public class MusicSingleCardDeckModel : IDisposable
    {
        private int cardDeckLevel;
        private ObservableList<MusicCardModel> cards = new ObservableList<MusicCardModel>();
        private IEnumerable<MusicCardModel> allCards;
        private IDisposable disposable;

        public MusicSingleCardDeckModel(int cardDeckLevel, IEnumerable<MusicCardModel> allCards)
        {
            this.cardDeckLevel = cardDeckLevel;
            this.allCards = allCards ?? throw new ArgumentNullException(nameof(allCards));

            InitializeDeck();
            SubscribeToCardStateChanges();
        }

        private void InitializeDeck()
        {
            var initialCards = allCards.Where(
                card => card.Level == cardDeckLevel && card.State.CurrentValue == MusicCardState.InDeck).ToList();
            
            cards.AddRange(initialCards);
        }

        public void SubscribeToCardStateChanges()
        {
            var d = Disposable.CreateBuilder();

            foreach (var card in allCards.Where(c => c.Level == cardDeckLevel))
            {
                card.State.Subscribe(state => HandleCardStateChange(card, state)).AddTo(ref d);
            }

            disposable = d.Build();
        }

        private void HandleCardStateChange(MusicCardModel card, MusicCardState state)
        {
            // TODO : Optimize this

            var isInDeck = cards.Contains(card);
            var shouldBeInDeck = state == MusicCardState.InDeck;

            if (shouldBeInDeck && !isInDeck)
            {
                cards.Add(card);
            }
            else if (!shouldBeInDeck && isInDeck)
            {
                cards.Remove(card);
            }
        }

        public MusicCardModel GetRandomCard()
        {
            if (cards.Count == 0) return null;

            var randomIndex = UnityEngine.Random.Range(0, cards.Count);
            return cards[randomIndex];
        }

        public void Dispose()
        {
            disposable?.Dispose();
        }
    }
}