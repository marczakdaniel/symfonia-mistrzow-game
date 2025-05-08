using UnityEngine;
using DG.Tweening;

public class CardStackSingleCardShowdownAnimation : MonoBehaviour
{
    [SerializeField]
    private Transform startPosition;

    [SerializeField]
    private Transform endPosition;

    [SerializeField]
    private GameObject cardGameObject;
    [SerializeField]
    private float animationTime;

    [SerializeField]
    private AnimationCurve animationCurve;

    private Tweener currentTween;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            Play();
        }
    }

    public void Play()
    {
        // Kill any existing tween
        currentTween?.Kill();

        // Reset position to start
        cardGameObject.transform.position = startPosition.position;

        // Create the movement tween
        currentTween = cardGameObject.transform
            .DOMove(endPosition.position, animationTime)
            .SetEase(animationCurve)
            .OnComplete(() => {
                currentTween.Rewind();
                currentTween = null;
            });
    }

    private void OnDestroy()
    {
        currentTween?.Kill();
    }
}