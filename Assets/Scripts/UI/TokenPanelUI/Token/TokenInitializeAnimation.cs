using Cysharp.Threading.Tasks;
using DG.Tweening;
using Mono.Cecil.Cil;
using UnityEngine;

public class TokenInitializeAnimation : MonoBehaviour
{
    [SerializeField] private RectTransform token;
    [SerializeField] private RectTransform tokenImage;
    [SerializeField] private RectTransform endPosition;
    [SerializeField] private RectTransform tokenEndPosition;
    [SerializeField] private AnimationCurve animationCurve;

    public async UniTask Play(float animationTime)
    {
        var isFinished = false;
        var seq = DOTween.Sequence();
        seq.Append(token.DOMove(endPosition.position, animationTime).SetEase(animationCurve));
        seq.Append(tokenImage.DOJump(tokenEndPosition.position, 30, 1, 0.5f));
        seq.OnComplete(() =>
        {
            isFinished = true;
        });

        await UniTask.WaitUntil(() => isFinished);
    }
}
