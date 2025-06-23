using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class TokenValueChangeAnimation : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI valueChangeText;
    [SerializeField]
    private Color positiveColor;
    [SerializeField]
    private Color negativeColor;
    [SerializeField]
    private CanvasGroup valueChangeCanvasGroup;
    [SerializeField]
    private RectTransform rectTransform;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            PlayValueChangeAnimation(10, true).Forget();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            PlayValueChangeAnimation(10, false).Forget();
        }
    }


    public async UniTask PlayValueChangeAnimation(int value, bool isPositive)
    { 
        var isFinished = false;

        valueChangeText.text = isPositive ? $"+{value}" : $"-{value}";
        valueChangeText.color = isPositive ? positiveColor : negativeColor;
        rectTransform.localScale = Vector3.one;
        rectTransform.localPosition = new Vector3(rectTransform.localPosition.x, 0, rectTransform.localPosition.z);
        valueChangeCanvasGroup.alpha = 1;

        Sequence seq = DOTween.Sequence();
        seq.Append(rectTransform.DOScale(Vector3.one * 0.9f, 0.5f));
        seq.Join(valueChangeCanvasGroup.DOFade(0, 0.5f));
        seq.Join(rectTransform.DOLocalMoveY(rectTransform.localPosition.y + 30, 0.5f));
        seq.OnComplete(() =>
        {
            isFinished = true;
        });        
        
        await UniTask.WaitUntil(() => isFinished);
    }
}
