namespace UI.ReturnTokenWindow.ReturnTokenPlayerTokensPanel
{
    public class ReturnTokenPlayerTokensPanelPresenter
    {
        private readonly ReturnTokenPlayerTokensPanelViewModel viewModel;
        private readonly ReturnTokenPlayerTokensPanelView view;

        public ReturnTokenPlayerTokensPanelPresenter( ReturnTokenPlayerTokensPanelView view)
        {
            this.viewModel = new ReturnTokenPlayerTokensPanelViewModel();
            this.view = view;
        }
    }
}