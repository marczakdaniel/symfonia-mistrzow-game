using Cysharp.Threading.Tasks;
using DefaultNamespace.UI.Card;
using DG.Tweening;
using UnityEngine;

public class CardsRowSingleCardShowdownAnimation : MonoBehaviour
{
    [SerializeField]    
    private RectTransform startRectTransform;

    [SerializeField]
    private RectTransform cardRectTransform;

    [SerializeField]
    private RectTransform targetRectTransform;

    [SerializeField]
    private float animationTime;

    [SerializeField]
    private AnimationCurve animationCurve;

    private Sequence sequence;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Play().Forget();
        }
    }

    public async UniTask Play()
    {
        // Kill any existing tween
        sequence?.Kill();

        // Reset position to start
        cardRectTransform.position = startRectTransform.position;

        var isFinished = false;
        // Create the movement tween

        sequence = DOTween.Sequence();

        sequence.Append(cardRectTransform
            .DOMove(targetRectTransform.position, animationTime)
            .SetEase(animationCurve));
        sequence.Join(cardRectTransform
            .DOSizeDelta(targetRectTransform.sizeDelta, animationTime)
            .SetEase(animationCurve));

        sequence.OnComplete(() => {
            sequence = null;
            isFinished = true;
        });
        
        await UniTask.WaitUntil(() => isFinished);
    }

    public void ResetAnimation()
    {
        sequence?.Kill();
        cardRectTransform.position = startRectTransform.position;
        cardRectTransform.sizeDelta = startRectTransform.sizeDelta;
    }

    private void OnDestroy()
    {
        sequence?.Kill();
    }

}