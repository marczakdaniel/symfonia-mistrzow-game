using R3;
using SymfoniaMistrzow.MVP.Common;
using UnityEngine;

namespace SymfoniaMistrzow.MVP.Card
{
    public class CardPresenter : IPresenter
    {
        private readonly CardModel _model;
        private readonly CardView _view;
        private readonly CompositeDisposable _disposables = new();

        public CardPresenter(CardModel model, CardView view)
        {
            _model = model;
            _view = view;
        }

        public void Initialize()
        {
            // Bind model properties to view updates using R3
            _model.Points
                .Subscribe(points => _view.SetPoints(points))
                .AddTo(_disposables);

            // _model.GemColor
            //    .Subscribe(color => _view.SetGemImage(GetSpriteForColor(color))) // Assuming a helper to get sprites
            //    .AddTo(_disposables);

            _view.BuyButton.onClick.AddListener(OnBuyButtonClicked);
        }

        private void OnBuyButtonClicked()
        {
            // Handle the buy logic, e.g., by raising an event
            // or calling a game logic service.
            Debug.Log("Buy button clicked for card.");
        }

        public void Dispose()
        {
            _disposables.Dispose();
            _view.BuyButton.onClick.RemoveListener(OnBuyButtonClicked);
        }
    }
} 