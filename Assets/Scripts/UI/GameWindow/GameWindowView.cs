using UI.Board;
using UI.MusicCardDetailsPanel;
using UI.SelectTokenWindow;
using UI.StartTurnWindow;
using UnityEngine;

namespace UI.GameWindow
{
    public class GameWindowView : MonoBehaviour
    {
        public BoardView BoardView => boardView;
        public MusicCardDetailsPanelView MusicCardDetailsPanelView => musicCardDetailsPanelView;
        public SelectTokenWindowView SelectTokenWindowView => selectTokenWindowView;
        public StartTurnWindowView StartTurnWindowView => startTurnWindowView;
        
        [SerializeField] private BoardView boardView;
        [SerializeField] private MusicCardDetailsPanelView musicCardDetailsPanelView;
        [SerializeField] private SelectTokenWindowView selectTokenWindowView;
        [SerializeField] private StartTurnWindowView startTurnWindowView;
    }
}