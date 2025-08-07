using DefaultNamespace.Data;

namespace UI.Board.BoardPlayerPanel.BoardPlayerPanelSingleResource
{
    public class BoardPlayerPanelSingleResourceViewModel
    {
        public string PlayerId { get; private set; }
        public ResourceType ResourceType { get; private set; }
        public int Index { get; private set; }

        public int CurrentTokenCount { get; private set; }
        public int CurrentCardCount { get; private set; }

        public BoardPlayerPanelSingleResourceViewModel(int index, ResourceType resourceType)
        {
            Index = index;
            ResourceType = resourceType;
        }

        public void SetPlayerId(string playerId)
        {
            PlayerId = playerId;
        }

        public void SetCurrentTokenCount(int tokenCount)
        {
            CurrentTokenCount = tokenCount;
        }

        public void SetCurrentCardCount(int cardCount)
        {
            CurrentCardCount = cardCount;
        }
    }
}