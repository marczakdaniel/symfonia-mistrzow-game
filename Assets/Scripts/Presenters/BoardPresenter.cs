using DefaultNamespace.Models;
using DefaultNamespace.Views;
using ObservableCollections;
using R3;

namespace DefaultNamespace.Presenters
{
    public class BoardPresenter 
    {
        private readonly BoardModel model;
        private readonly BoardView view;

        public BoardPresenter(BoardModel boardModel, BoardView boardView)
        {
            this.model = boardModel;
            this.view = boardView;

            ConnectModel();
        }

        private void ConnectModel()
        {
            //model.Level1Cards.ObserveAdd().Sub
        }
    }
}