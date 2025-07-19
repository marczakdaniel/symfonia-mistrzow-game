using R3;

namespace UI.SelectTokenWindow
{
    public enum SelectTokenWindowState
    {
        Disabled,
        DuringEntryAnimation,
        Active,
    }

    public class SelectTokenWindowViewModel
    {
        public ReactiveProperty<SelectTokenWindowState> State { get; private set; } = new ReactiveProperty<SelectTokenWindowState>(SelectTokenWindowState.Disabled);

        public SelectTokenWindowViewModel()
        {
            
        }

        public void SetState(SelectTokenWindowState state)
        {
            State.Value = state;
        }
    }
}