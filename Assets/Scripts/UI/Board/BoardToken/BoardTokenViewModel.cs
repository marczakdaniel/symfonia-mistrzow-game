using System;
using DefaultNamespace.Data;
using R3;
using UnityEngine;
namespace UI.Board.BoardTokenPanel.BoardToken
{
    public enum BoardTokenState
    {
        Disabled,
        DuringEntryAnimation,
        DuringAddingTokens,
        Active,
        DuringTokenDetailsPanelOpen,
        DuringReturnTokensPanelOpen,
    }

    public class BoardTokenViewModel
    {
        public ReactiveProperty<BoardTokenState> State { get; private set; } = new ReactiveProperty<BoardTokenState>(BoardTokenState.Disabled);
        public ResourceType ResourceType { get; private set; }
        public int TokenCount { get; private set; } = 0;

        public BoardTokenViewModel(ResourceType resourceType)
        {
            ResourceType = resourceType;
        }

        public void SetTokenCount(int tokenCount)
        {
            TokenCount = tokenCount;
        }

        // State change

        private void SetState(BoardTokenState state)
        {
            State.Value = state;
        }
        public bool OnEntry(int numberOfTokens)
        {
            if (!CanOnEntry()) {
                Debug.LogError($"[BoardToken] Cannot on entry in state: {State.Value}");
                return false;
            }

            SetTokenCount(numberOfTokens);
            SetState(BoardTokenState.DuringEntryAnimation);
            return true;
        }

        private bool CanOnEntry()
        {
            return State.Value == BoardTokenState.Disabled;
        }

        public bool CompleteEntryAnimation()    
        {
            SetState(BoardTokenState.Active);
            return true;
        }
        public bool OpenTokenDetailsPanel()
        {
            if (!CanOpenTokenDetailsPanel()) {
                Debug.LogError($"[BoardToken] Cannot open token details panel in state: {State.Value}");
                return false;
            }

            SetState(BoardTokenState.DuringTokenDetailsPanelOpen);
            return true;
        }

        private bool CanOpenTokenDetailsPanel()
        {
            return State.Value == BoardTokenState.Active;
        }

        public bool CloseTokenDetailsPanel()
        {
            if (!CanCloseTokenDetailsPanel()) {
                Debug.LogError($"[BoardToken] Cannot close token details panel in state: {State.Value}");
                return false;
            }

            SetState(BoardTokenState.Active);
            return true;
        }

        private bool CanCloseTokenDetailsPanel()
        {
            return State.Value == BoardTokenState.DuringTokenDetailsPanelOpen;
        }

        public bool OnConfirmSelectedTokens(int numberOfTokens)
        {
            if (!CanOnConfirmSelectedTokens()) {
                Debug.LogError($"[BoardToken] Cannot confirm selected tokens in state: {State.Value}");
                return false;
            }

            SetTokenCount(numberOfTokens);
            SetState(BoardTokenState.DuringAddingTokens);
            return true;
        }

        private bool CanOnConfirmSelectedTokens()
        {
            return State.Value == BoardTokenState.DuringTokenDetailsPanelOpen;
        }

        public bool OpenReturnTokensPanel()
        {
            if (!CanOpenReturnTokensPanel()) {
                Debug.LogError($"[BoardToken] Cannot open return tokens panel in state: {State.Value}");
                return false;
            }

            SetState(BoardTokenState.DuringReturnTokensPanelOpen);
            return true;
        }

        private bool CanOnReturnTokensConfirmed()
        {
            return State.Value == BoardTokenState.DuringReturnTokensPanelOpen;
        }

        private bool CanOpenReturnTokensPanel()
        {
            return State.Value == BoardTokenState.Active;
        }

        public bool OnReturnTokensConfirmed(int numberOfTokens)
        {
            if (!CanOnReturnTokensConfirmed()) {
                Debug.LogError($"[BoardToken] Cannot return tokens confirmed in state: {State.Value}");
                return false;
            }

            SetTokenCount(numberOfTokens);
            SetState(BoardTokenState.Active);
            return true;
        }

        public bool AddTokens()
        {
            if (!CanAddTokens()) {
                Debug.LogError($"[BoardToken] Cannot add tokens in state: {State.Value}");
                return false;
            }

            SetState(BoardTokenState.DuringAddingTokens);
            return true;
        }

        private bool CanAddTokens()
        {
            return State.Value == BoardTokenState.Active;
        }

        public bool CompleteAddingTokens()
        {
            if (!CanCompleteAddingTokens()) {
                Debug.LogError($"[BoardToken] Cannot complete moving tokens in state: {State.Value}");
                return false;
            }

            SetState(BoardTokenState.Active);
            return true;
        }

        private bool CanCompleteAddingTokens()
        {
            return State.Value == BoardTokenState.DuringAddingTokens;
        }
    }
}