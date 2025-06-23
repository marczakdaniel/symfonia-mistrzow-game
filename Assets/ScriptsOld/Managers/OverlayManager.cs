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

        public void OpenCardActionOverlay(CardActionOverlayData data)
        {
            var model = new CardActionOverlayModel(data.CardData);
            var controller = new CardActionOverlayController(model, cardActionOverlayView);
            controller.OnCardBuy += data.OnCardBuy;
            controller.OnCardReserve += data.OnCardReserve;
            controller.OnOverlayClose += () =>
            {
                controller.OnCardBuy -= data.OnCardBuy;
                controller.OnCardReserve -= data.OnCardReserve;
            };
        }
    }
}