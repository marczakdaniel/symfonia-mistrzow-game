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
        public string PlayerId { get; private set; }
        public Sprite PlayerImage { get; private set; }
        public int Index { get; private set; }
        public bool IsCurrentPlayer { get; private set; }

        public BoardPlayerPanelViewModel(int index)
        {
            Index = index;
        }

        public void Initialize(string playerId, Sprite playerImage)
        {
            PlayerId = playerId;
            PlayerImage = playerImage;
            IsCurrentPlayer = false;
        }

        public void SetCurrentPlayer(bool isCurrentPlayer)
        {
            IsCurrentPlayer = isCurrentPlayer;
        }
    }
}