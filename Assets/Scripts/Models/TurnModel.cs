using System.Collections;
using System.Collections.Generic;
using DefaultNamespace.Data;
using UnityEngine;

namespace Models
{
    public enum TurnState
    {
        WaitingForAction,
        SelectingTokens,
        SelectingMusicCard,
        ConfirmingAction,
    }

    public interface ITurnModelReader
    {
        public ResourceType?[] GetSelectedTokens();
    }

    public class TurnModel : ITurnModelReader
    {
        public string CurrentPlayerId {get; private set; }
        public TurnState State {get; private set; }
        public List<ResourceType> SelectedTokens {get; private set; } = new List<ResourceType>();

        public TurnModel()
        {
            SelectedTokens = new List<ResourceType>();
        }

        public void SetState(TurnState state)
        {
            State = state;
        }

        public void AddTokenToSelectedTokens(ResourceType token)
        {
            SelectedTokens.Add(token);
        }

        public void RemoveTokenFromSelectedTokens(ResourceType token)
        {
            SelectedTokens.Remove(token);
        }

        public void ClearSelectedTokens()
        {
            SelectedTokens.Clear();
        }

        public ResourceType?[] GetSelectedTokens()
        {
            var result = new ResourceType?[3] { null, null, null };  
            var i = 0;
            for (i = 0; i < result.Length; i++)
            {
                if (i < SelectedTokens.Count)
                {
                    result[i] = SelectedTokens[i];
                }
                else
                {
                    break;
                }
            }
            return result;
        }

    }
}