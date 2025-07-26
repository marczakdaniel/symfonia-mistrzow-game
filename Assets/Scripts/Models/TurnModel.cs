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
        ReadyToEndTurn,
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
        public List<ResourceType> ReturnTokens {get; private set; } = new List<ResourceType>();
        public bool CanAddTokenToSelectedTokens(ResourceType token)
        {
            if (SelectedTokens.Count >= 3) return false;
            if (SelectedTokens.Contains(token) && SelectedTokens.Count >= 2) return false;
            if (SelectedTokens.Count >= 2 && SelectedTokens[0] == SelectedTokens[1]) return false;
            return true;
        }

        public TurnModel()
        {
            SelectedTokens = new List<ResourceType>();
        }

        public void SetState(TurnState state)
        {
            State = state;
        }

        public void SetCurrentPlayer(string playerId)
        {
            CurrentPlayerId = playerId;

        }

        // Selected Tokens

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

        public int GetSelectedTokensCount(ResourceType token)
        {
            var result = 0;
            foreach (var selectedToken in SelectedTokens)
            {
                if (selectedToken == token)
                {
                    result++;
                }
            }
            return result;
        }

        public ResourceCollectionModel GetSelectedTokensCollection()
        {
            return new ResourceCollectionModel(SelectedTokens.ToArray());
        }

        // Return Tokens

        public void AddTokenToReturnTokens(ResourceType token)
        {
            ReturnTokens.Add(token);
        }

        public void RemoveTokenFromReturnTokens(ResourceType token)
        {
            ReturnTokens.Remove(token);
        }

        public void ClearReturnTokens()
        {
            ReturnTokens.Clear();
        }

        public ResourceCollectionModel GetReturnTokensCollection()
        {
            return new ResourceCollectionModel(ReturnTokens.ToArray());
        }

        
    }
}