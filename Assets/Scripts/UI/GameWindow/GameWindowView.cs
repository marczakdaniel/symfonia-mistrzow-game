using UI.Board;
using UI.MusicCardDetailsPanel;
using UnityEngine;

namespace UI.GameWindow
{
    public class GameWindowView : MonoBehaviour
    {
        public BoardView BoardView => boardView;
        public MusicCardDetailsPanelView MusicCardDetailsPanelView => musicCardDetailsPanelView;

        [SerializeField] private BoardView boardView;
        [SerializeField] private MusicCardDetailsPanelView musicCardDetailsPanelView;
    }
}