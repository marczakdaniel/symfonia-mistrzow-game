using R3;
using UnityEngine;

namespace UI.Board.BoardPlayerPanel
{
    public enum BoardPlayerPanelState
    {
        Disabled,
        Enabled,
        CurrentPlayer,
    }

    public class BoardPlayerPanelViewModel
    {
        public ReactiveProperty<BoardPlayerPanelState> State { get; private set; } = new ReactiveProperty<BoardPlayerPanelState>(BoardPlayerPanelState.Disabled);
        public ReactiveProperty<int> PlayerPoints { get; private set; } = new ReactiveProperty<int>(0);
        public string PlayerId { get; private set; }
        public Sprite PlayerImage { get; private set; }
        public int Index { get; private set; }

        public BoardPlayerPanelViewModel(int index)
        {
            Index = index;
        }

        public void Initialize(string playerId, int points, Sprite playerImage)
        {
            PlayerId = playerId;
            PlayerPoints.Value = 0;
            PlayerImage = playerImage;
            State.Value = BoardPlayerPanelState.Enabled;
        }

        public void SetCurrentPlayer(bool isCurrentPlayer)
        {
            State.Value = isCurrentPlayer ? BoardPlayerPanelState.CurrentPlayer : BoardPlayerPanelState.Enabled;
        }

        public void SetPlayerPoints(int points)
        {
            PlayerPoints.Value = points;
        }
    }
}