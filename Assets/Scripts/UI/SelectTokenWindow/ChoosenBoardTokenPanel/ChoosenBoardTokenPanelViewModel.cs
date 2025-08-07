using System.Collections.Generic;
using DefaultNamespace.Data;
using R3;

namespace UI.SelectTokenWindow.ChoosenBoardTokenPanel
{
public class ChoosenBoardTokenPanelViewModel
    {
        public ResourceType?[] SelectedTokens { get; private set; } = new ResourceType?[3] { null, null, null };

        public void OnOpenAnimation(ResourceType? selectedToken)
        {
            SelectedTokens[0] = selectedToken.HasValue ? selectedToken.Value : null;
            SelectedTokens[1] = null;
            SelectedTokens[2] = null;
        }

        public void OnCloseAnimation()
        {
            SelectedTokens[0] = null;
            SelectedTokens[1] = null;
            SelectedTokens[2] = null;
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
    }
}