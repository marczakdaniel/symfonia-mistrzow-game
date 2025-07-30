using System;
using DefaultNamespace.Data;
using R3;
using ObservableCollections;
using System.Collections.Generic;

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
        public ResourceType? SelectedResourceType { get; private set; }
        public Dictionary<ResourceType, int> PlayerTokens { get; private set; }
        
        public SelectTokenWindowViewModel()
        {
            PlayerTokens = new Dictionary<ResourceType, int>();
        }

        public void SetState(SelectTokenWindowState state)
        {
            State.Value = state;
        }

        public void SetSelectedResourceType(ResourceType? resourceType)
        {
            SelectedResourceType = resourceType.HasValue ? resourceType.Value : null;
        }

        public void SetPlayerTokens(Dictionary<ResourceType, int> playerTokens)
        {
            PlayerTokens = playerTokens;
        }

        public void OpenWindow(ResourceType? resourceType, Dictionary<ResourceType, int> playerTokens)
        {
            SetSelectedResourceType(resourceType);
            SetPlayerTokens(playerTokens);
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