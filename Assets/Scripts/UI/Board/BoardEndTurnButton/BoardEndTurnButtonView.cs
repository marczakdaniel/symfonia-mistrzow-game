using DefaultNamespace.Elements;
using R3;
using UnityEngine;

namespace UI.Board.BoardEndTurnButton
{
    public class BoardEndTurnButtonView : MonoBehaviour
    {
        public Subject<Unit> OnButtonClicked { get; private set; } = new Subject<Unit>();

        [SerializeField] private ButtonElement button;

        public void Awake()
        {
            button.OnClick.Subscribe(_ => OnButtonClicked.OnNext(Unit.Default)).AddTo(this);
        }
    }
}