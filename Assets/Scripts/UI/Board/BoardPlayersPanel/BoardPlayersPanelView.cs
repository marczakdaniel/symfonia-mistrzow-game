using UI.Board.BoardPlayersPanel.BoardSinglePlayer;
using UnityEngine;

namespace UI.Board.BoardPlayersPanel
{
    public class BoardPlayersPanelView : MonoBehaviour
    {
        [SerializeField] private BoardSinglePlayerView[] boardSinglePlayerViews = new BoardSinglePlayerView[4];

        public BoardSinglePlayerView[] BoardSinglePlayerViews => boardSinglePlayerViews;
    }
}