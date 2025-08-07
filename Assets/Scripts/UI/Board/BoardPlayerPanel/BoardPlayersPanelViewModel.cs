using R3;
using UnityEngine;

namespace UI.Board.BoardPlayerPanel
{
    public class BoardPlayerPanelViewModel
    {
        public string PlayerId { get; private set; }
        public Sprite PlayerImage { get; private set; }
        public int Index { get; private set; }
        public bool IsCurrentPlayer { get; private set; }
        public int Points { get; private set; }

        public BoardPlayerPanelViewModel(int index)
        {
            Index = index;
        }

        public void Initialize(string playerId, Sprite playerImage)
        {
            PlayerId = playerId;
            PlayerImage = playerImage;
            IsCurrentPlayer = false;
            Points = 0;
        }

        public void SetCurrentPlayer(bool isCurrentPlayer)
        {
            IsCurrentPlayer = isCurrentPlayer;
        }

        public void SetPoints(int points)
        {
            Points = points;
        }
    }
}