using DefaultNamespace.Data;
using R3;

namespace UI.SelectTokenWindow.SelectSingleToken
{
    public enum SelectSingleTokenState
    {
        Disabled,
        DuringOpenAnimation,
        DuringCloseAnimation,
        DuringAddingTokenAnimation,
        DuringRemovingTokenAnimation,
        Active,
    }
    public class SelectSingleTokenViewModel
    {
        public ReactiveProperty<SelectSingleTokenState> State { get; private set; } = new ReactiveProperty<SelectSingleTokenState>(SelectSingleTokenState.Disabled);
        public ResourceType ResourceType { get; private set; }
        public int Count { get; private set; } = 0;

        public SelectSingleTokenViewModel(ResourceType resourceType)
        {
            ResourceType = resourceType;
        }

        public void OnOpenWindow(int count)
        {
            SetCount(count);
            SetState(SelectSingleTokenState.DuringOpenAnimation);
        }

        public void OnCloseWindow()
        {
            SetState(SelectSingleTokenState.DuringCloseAnimation);
        }

        public void AddToken(int newValue)
        {
            SetCount(newValue);
            SetState(SelectSingleTokenState.DuringAddingTokenAnimation);
        }

        public void RemoveToken(int newValue)
        {
            SetCount(newValue);
            SetState(SelectSingleTokenState.DuringRemovingTokenAnimation);
        }

        public void OnOpenAnimationFinished()
        {
            SetState(SelectSingleTokenState.Active);
        }

        public void OnCloseAnimationFinished()
        {
            SetState(SelectSingleTokenState.Disabled);
        }

        public void OnAddingTokenAnimationFinished()
        {
            SetState(SelectSingleTokenState.Active);
        }

        public void OnRemovingTokenAnimationFinished()
        {
            SetState(SelectSingleTokenState.Active);
        }   

        private void SetState(SelectSingleTokenState state)
        {
            State.Value = state;
        }

        private void SetCount(int newValue)
        {
            Count = newValue;
        }
    }
}