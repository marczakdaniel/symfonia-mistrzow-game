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

    public class TurnModel
    {
        public string CurrentPlayerId {get; private set; }
        public TurnState State {get; private set; }
        public Stack<ResourceType?> SelectedTokens {get; private set; } = new Stack<ResourceType?>();

        public TurnModel()
        {
            SelectedTokens = new Stack<ResourceType?>();
        }

        public void SetState(TurnState state)
        {
            State = state;
        }

        public void AddTokenToSelectedTokens(ResourceType token)
        {
            if (State != TurnState.SelectingTokens)
            {
                Debug.LogError("[TurnModel] Cannot add token to selected tokens in this state");
                return;
            }

            SelectedTokens.Push(token);
        }

    }
}