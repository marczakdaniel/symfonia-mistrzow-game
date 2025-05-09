using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class BoardAnimationController : MonoBehaviour
{
    [SerializeField]
    private CardsRowAnimationController[] cardsRowAnimationControllers;

    [Header("All Cards Showdown Animation")]
    [SerializeField]
    private float allCardsShowdownAnimationDelay = 0.5f;

    [SerializeField]
    private float allCardsFlipAnimationDelay = 0.5f;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            PlayAllShowdownAnimation().Forget();
        }
    }

    public async UniTask PlayInitializeAnimation()
    {
        
    }

    public async UniTask PlayAllShowdownAnimation()
    {
        var tasks = new List<UniTask>(cardsRowAnimationControllers.Length);
        foreach (var controller in cardsRowAnimationControllers)
        {
            tasks.Add(controller.PlayAllCardsShowdownAnimation(allCardsShowdownAnimationDelay));
            await UniTask.Delay(TimeSpan.FromSeconds(allCardsShowdownAnimationDelay * 4));
        }
        await UniTask.WhenAll(tasks);
    }

    public void ResetAllCardsShowdownAnimation()
    {
        foreach (var controller in cardsRowAnimationControllers)
        {
             controller.ResetAllCardsShowdownAnimation();
        }
    }
    public async UniTask PlayAllFlipAnimation()
    {
        var tasks = new List<UniTask>(cardsRowAnimationControllers.Length);
        foreach (var controller in cardsRowAnimationControllers)
        {
            tasks.Add(controller.PlayAllCardsFlipAnimation(allCardsFlipAnimationDelay));
            await UniTask.Delay(TimeSpan.FromSeconds(allCardsFlipAnimationDelay));
        }
        await UniTask.WhenAll(tasks);
    }

    public async UniTask PlayShowdownAnimation()
    {
        
    }

    public async UniTask PlayFlipAnimation()
    {

    }
}