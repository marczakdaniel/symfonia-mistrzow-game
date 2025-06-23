using R3;
using SymfoniaMistrzow.MVP.Common;

namespace SymfoniaMistrzow.MVP.Player
{
    public class PlayerPresenter : IPresenter
    {
        private readonly PlayerModel _model;
        private readonly PlayerView _view;
        private readonly CompositeDisposable _disposables = new();

        public PlayerPresenter(PlayerModel model, PlayerView view)
        {
            _model = model;
            _view = view;
        }

        public void Initialize()
        {
            _model.Name
                .Subscribe(_view.SetName)
                .AddTo(_disposables);

            _model.Points
                .Subscribe(_view.SetPoints)
                .AddTo(_disposables);

            // Here you would bind token counts to the view
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
} 