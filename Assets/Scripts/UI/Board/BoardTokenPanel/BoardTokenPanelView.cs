using UI.Board.BoardTokenPanel.BoardToken;
using UnityEngine;

namespace UI.Board.BoardTokenPanel
{
    public class BoardTokenPanelView : MonoBehaviour
    {
        public BoardTokenView[] Tokens => tokens;
        [SerializeField] private BoardTokenView[] tokens;
    }
}