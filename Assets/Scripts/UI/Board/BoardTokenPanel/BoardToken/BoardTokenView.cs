using UnityEngine;
using UnityEngine.UI;
using TMPro;
using R3;
using UnityEngine.EventSystems;
using Cysharp.Threading.Tasks;
namespace UI.Board.BoardTokenPanel.BoardToken
{
    public class BoardTokenView : MonoBehaviour, IPointerClickHandler
    {
        public Subject<Unit> OnTokenClicked { get; private set; } = new Subject<Unit>();

        [SerializeField] private Image tokenImage;  
        [SerializeField] private TextMeshProUGUI tokenCountText;
        [SerializeField] private BoardTokenEntryAnimation entryAnimation;

        public void DisableElement()
        {
            gameObject.SetActive(false);
        }

        public UniTask PlayEntryAnimation()
        {
            gameObject.SetActive(true);
            return entryAnimation.Play();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            OnTokenClicked?.OnNext(Unit.Default);
        }
    }
}