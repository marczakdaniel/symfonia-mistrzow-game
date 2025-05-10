using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class TokenPanelInitializeAnimation : MonoBehaviour
{
    [SerializeField] private RectTransform tokenPanel;

    public async UniTask InitializeTokenPanel()
    {
        var isFinished = false;
        var seq = DOTween.Sequence();
        seq.Append(tokenPanel.DOLocalMoveY(0f, 1.5f).SetEase(Ease.OutBack));
        seq.OnComplete(() =>
        {
            isFinished = true;
        });

        await UniTask.WaitUntil(() => isFinished);
    }
}
