using System;
using DefaultNamespace.Data;
using R3;
using ObservableCollections;

namespace UI.SelectTokenWindow
{
    public enum SelectTokenWindowState
    {
        Closed,
        Active,
        DuringOpenAnimation,
        DuringCloseAnimation,
    }


    public class SelectTokenWindowViewModel
    {
        public ReactiveProperty<SelectTokenWindowState> State { get; private set; } = new ReactiveProperty<SelectTokenWindowState>(SelectTokenWindowState.Closed);
        public ResourceType SelectedResourceType { get; private set; }
        
        public SelectTokenWindowViewModel()
        {

        }

        public void SetState(SelectTokenWindowState state)
        {
            State.Value = state;
        }

        public void SetSelectedResourceType(ResourceType resourceType)
        {
            SelectedResourceType = resourceType;
        }

        public void OpenWindow(ResourceType resourceType)
        {
            SetSelectedResourceType(resourceType);
            SetState(SelectTokenWindowState.DuringOpenAnimation);
        }

        public void OnOpenWindowFinished()
        {
            SetState(SelectTokenWindowState.Active);
        }

        public void CloseWindow()
        {
            SetState(SelectTokenWindowState.DuringCloseAnimation);
        }

        public void OnCloseWindowFinished()
        {
            SetState(SelectTokenWindowState.Closed);
        }
    }
}