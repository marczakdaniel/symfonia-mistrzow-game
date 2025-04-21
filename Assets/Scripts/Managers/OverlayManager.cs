using DefaultNamespace.UI.CardActionOverlay;
using DefaultNamespace.UI.Overlay;
using UnityEngine;

namespace DefaultNamespace.Managers
{
    public class OverlayManager : MonoBehaviour
    {
        [SerializeField] private CardActionOverlayView cardActionOverlayView;
        
        public OverlayManager()
        {
            
        }

        public void InitializeOverlayMVC()
        {
            
        }

        public void ShowOverlay(IOverlay overlay)
        {
            
        }

        public void OpenCardActionOverlay(CardData cardData)
        {
            var model = new CardActionOverlayModel(cardData);
            var controller = new CardActionOverlayController(model, cardActionOverlayView);
        }
    }
}