using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class TokenPanelInitializeAnimation : MonoBehaviour
{
    [SerializeField] private TokenInitializeAnimation[] tokenInitializeAnimations;

    public async UniTask InitializeTokenPanel()
    {
        var tasks = new List<UniTask>();

        foreach (var tokenInitializeAnimation in tokenInitializeAnimations)
        {
            tasks.Add(tokenInitializeAnimation.Play(1.2f));
            await UniTask.Delay(TimeSpan.FromSeconds(0.4f));
        }

        await UniTask.WhenAll(tasks);
    }
}
