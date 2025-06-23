using R3;
using SymfoniaMistrzow.MVP.Common;
using SymfoniaMistrzow.MVP.Card;
using UnityEngine;

namespace SymfoniaMistrzow.MVP.Board
{
    public class BoardPresenter : IPresenter
    {
        private readonly BoardModel _model;
        private readonly BoardView _view;
        private readonly CompositeDisposable _disposables = new();

        public BoardPresenter(BoardModel model, BoardView view)
        {
            _model = model;
            _view = view;
        }

        public void Initialize()
        {
            // When a card is added to the model, instantiate a view for it
            _model.Tier1Cards.ObserveAdd()
                .Subscribe(cardEvent => CreateCardView(cardEvent.Value, _view.Tier1CardContainer))
                .AddTo(_disposables);

             _model.Tier2Cards.ObserveAdd()
                .Subscribe(cardEvent => CreateCardView(cardEvent.Value, _view.Tier2CardContainer))
                .AddTo(_disposables);

             _model.Tier3Cards.ObserveAdd()
                .Subscribe(cardEvent => CreateCardView(cardEvent.Value, _view.Tier3CardContainer))
                .AddTo(_disposables);
        }

        private void CreateCardView(CardModel cardModel, Transform parent)
        {
            var cardObject = Object.Instantiate(_view.CardPrefab, parent);
            var cardView = cardObject.GetComponent<CardView>();
            // Here you would create and initialize the CardPresenter
            var cardPresenter = new CardPresenter(cardModel, cardView);
            cardPresenter.Initialize();
            // We might need to keep track of presenters to dispose them
        }

        public void Dispose()
        {
            _disposables.Dispose();
            // Dispose all card presenters
        }
    }
} 