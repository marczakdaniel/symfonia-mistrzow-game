using System.Collections.Generic;
using DefaultNamespace.Data;
using R3;

namespace UI.SelectTokenWindow.ChoosenBoardTokenPanel
{
    public enum ChoosenBoardTokenPanelState
    {
        Disabled,
        DuringOpenAnimation,
        DuringCloseAnimation,
        DuringAddingTokenAnimation,
        DuringRemovingTokenAnimation,
        Active,
    }

    public class ChoosenBoardTokenPanelViewModel
    {
        public ReactiveProperty<ChoosenBoardTokenPanelState> State { get; private set; } = new ReactiveProperty<ChoosenBoardTokenPanelState>(ChoosenBoardTokenPanelState.Disabled);
        public ResourceType?[] SelectedTokens { get; private set; } = new ResourceType?[3] { null, null, null };

        public void OnOpenAnimation(ResourceType selectedToken)
        {
            SelectedTokens[0] = selectedToken;
            SelectedTokens[1] = null;
            SelectedTokens[2] = null;

            SetState(ChoosenBoardTokenPanelState.DuringOpenAnimation);
        }

        public void OnCloseAnimation()
        {
            SelectedTokens[0] = null;
            SelectedTokens[1] = null;
            SelectedTokens[2] = null;

            SetState(ChoosenBoardTokenPanelState.DuringCloseAnimation);
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

            SetState(ChoosenBoardTokenPanelState.DuringAddingTokenAnimation);
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

            SetState(ChoosenBoardTokenPanelState.DuringRemovingTokenAnimation);
        }

        public void OnOpenAnimationFinished()
        {
            SetState(ChoosenBoardTokenPanelState.Active);
        }

        public void OnCloseAnimationFinished()
        {
            SetState(ChoosenBoardTokenPanelState.Disabled);
        }

        public void OnAddingTokenAnimationFinished()
        {
            SetState(ChoosenBoardTokenPanelState.Active);
        }

        public void OnRemovingTokenAnimationFinished()
        {
            SetState(ChoosenBoardTokenPanelState.Active);
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

        private void SetState(ChoosenBoardTokenPanelState state)
        {
            State.Value = state;
        }
    }
}