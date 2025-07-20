using System;   
using R3;

namespace UI.SelectTokenWindow
{
    public enum SelectTokenWindowState
    {
        Closed,
        Active,
        DuringOpenAnimation,
    }

    public class SelectTokenWindowViewModel
    {
        public ReactiveProperty<SelectTokenWindowState> State { get; private set; } = new ReactiveProperty<SelectTokenWindowState>(SelectTokenWindowState.Closed);

        public SelectTokenWindowViewModel()
        {
            
        }

        public void SetState(SelectTokenWindowState state)
        {
            State.Value = state;
        }

        public void OpenWindow()
        {
            SetState(SelectTokenWindowState.DuringOpenAnimation);
        }

        public void OnOpenWindowFinished()
        {
            SetState(SelectTokenWindowState.Active);
        }
    }
}