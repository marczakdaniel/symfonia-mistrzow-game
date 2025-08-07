using DefaultNamespace.Data;

namespace UI.SelectTokenWindow.SelectBoardToken
{
    public class SelectBoardTokenViewModel
    {
        public ResourceType ResourceType { get; private set; }
        
        public SelectBoardTokenViewModel(ResourceType resourceType)
        {
            ResourceType = resourceType;
        }
    }
}