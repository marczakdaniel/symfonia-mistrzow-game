using UnityEngine;
using TMPro;

namespace UI.Board.BoardPlayersPanel.BoardSinglePlayer
{
    public class BoardSinglePlayerView : MonoBehaviour
    {
        [SerializeField] private GameObject currentPlayerIndicator;

        public void SetIsCurrentPlayer(bool isCurrentPlayer)
        {
            currentPlayerIndicator.SetActive(isCurrentPlayer);
        }
    }
}