using DefaultNamespace.Data;
using R3;

namespace UI.ReturnTokenWindow.ReturnTokenSelectedPanel
{
    public enum ReturnTokenSelectedPanelState
    {
        Disabled,
        DuringOpenAnimation,
        DuringCloseAnimation,
        DuringAddingTokenAnimation,
        DuringRemovingTokenAnimation,
        Active,
    }

    public class ReturnTokenSelectedPanelViewModel
    {
        public ReactiveProperty<ReturnTokenSelectedPanelState> State { get; private set; } = new ReactiveProperty<ReturnTokenSelectedPanelState>(ReturnTokenSelectedPanelState.Disabled);
        public ResourceType?[] SelectedTokens { get; private set; } = new ResourceType?[3] { null, null, null };

        public void OnOpenAnimation()
        {
            SelectedTokens[0] = null;
            SelectedTokens[1] = null;
            SelectedTokens[2] = null;

            SetState(ReturnTokenSelectedPanelState.DuringOpenAnimation);
        }

        public void OnCloseAnimation()
        {
            SelectedTokens[0] = null;
            SelectedTokens[1] = null;
            SelectedTokens[2] = null;

            SetState(ReturnTokenSelectedPanelState.DuringCloseAnimation);
        }

        public void AddToken(ResourceType?[] tokens)
        {
            for (int i = 0; i < SelectedTokens.Length; i++)
            {
                if (tokens.Length <= i)
                {
                    SelectedTokens[i] = null;
                    continue;
                }

                SelectedTokens[i] = tokens[i];
            }

            SetState(ReturnTokenSelectedPanelState.DuringAddingTokenAnimation);
        }

        public void RemoveToken(ResourceType?[] tokens)
        {
            for (int i = 0; i < SelectedTokens.Length; i++)
            {
                if (tokens.Length <= i)
                {
                    SelectedTokens[i] = null;
                    continue;
                }
                SelectedTokens[i] = tokens[i];
            }

            SetState(ReturnTokenSelectedPanelState.DuringRemovingTokenAnimation);
        }

        public void OnOpenAnimationFinished()
        {
            SetState(ReturnTokenSelectedPanelState.Active);
        }

        public void OnCloseAnimationFinished()
        {
            SetState(ReturnTokenSelectedPanelState.Disabled);
        }

        public void OnAddingTokenAnimationFinished()
        {
            SetState(ReturnTokenSelectedPanelState.Active);
        }

        public void OnRemovingTokenAnimationFinished()
        {
            SetState(ReturnTokenSelectedPanelState.Active);
        }

        public ResourceType? GetLastSelectedToken()
        {
            for (int i = 2; i >= 0; i--)
            {
                if (SelectedTokens[i] != null)
                {
                    return SelectedTokens[i];
                }
            }
            return null;
        }

        private void SetState(ReturnTokenSelectedPanelState state)
        {
            State.Value = state;
        }
    }
}