using System.Collections.Generic;

namespace UI.ResultWindow
{
    public class ResultWindowViewModel
    {
        public List<string> PlayerIds { get; private set; }

        public void SetPlayerIds(List<string> playerIds)
        {
            PlayerIds = playerIds;
        }
    }
}