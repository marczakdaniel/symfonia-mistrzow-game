using DefaultNamespace.Data;
using R3;

namespace UI.SelectTokenWindow.SelectSingleToken
{
    public enum SelectSingleTokenState
    {
        Disabled,
        Active,
    }
    public class SelectSingleTokenViewModel
    {
        public ReactiveProperty<SelectSingleTokenState> State { get; private set; } = new ReactiveProperty<SelectSingleTokenState>(SelectSingleTokenState.Disabled);
        public ResourceType ResourceType { get; private set; }
        public ReactiveProperty<int> Count { get; private set; } = new ReactiveProperty<int>(0);

        public SelectSingleTokenViewModel(ResourceType resourceType)
        {
            ResourceType = resourceType;
        }

        public void OnOpenWindow(int count)
        {
            State.Value = SelectSingleTokenState.Active;
            Count.Value = count;
        }

        public void SetState(SelectSingleTokenState state)
        {
            State.Value = state;
        }


    }
}