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
        Active,
        DuringAddingTokens,
        DuringRemovingTokens,
        DuringTokenDetailsPanelOpen,
    }

    public class BoardTokenViewModel
    {
        public ReactiveProperty<BoardTokenState> State { get; private set; } = new ReactiveProperty<BoardTokenState>(BoardTokenState.Disabled);
        public ResourceType ResourceType { get; private set; }
        public ReactiveProperty<int> TokenCount { get; private set; } = new ReactiveProperty<int>(0);

        public BoardTokenViewModel(ResourceType resourceType)
        {
            ResourceType = resourceType;
        }

        public void SetTokenCount(int tokenCount)
        {
            TokenCount.Value = tokenCount;
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
            if (!CanCompleteEntryAnimation()) {
                Debug.LogError($"[BoardToken] Cannot complete entry animation in state: {State.Value}");
                return false;
            }

            SetState(BoardTokenState.Active);
            return true;
        }

        private bool CanCompleteEntryAnimation()
        {
            return State.Value == BoardTokenState.DuringEntryAnimation;
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

        public bool RemoveTokens()
        {
            if (!CanRemoveTokens()) {
                Debug.LogError($"[BoardToken] Cannot remove tokens in state: {State.Value}");
                return false;
            }

            SetState(BoardTokenState.DuringRemovingTokens);
            return true;
        }

        private bool CanRemoveTokens()
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
            return State.Value == BoardTokenState.DuringAddingTokens || State.Value == BoardTokenState.DuringRemovingTokens;
        }
    }
}