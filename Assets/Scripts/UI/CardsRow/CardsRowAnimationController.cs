using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DefaultNamespace.UI.Card;
using UnityEngine;

public class CardsRowAnimationController : MonoBehaviour
{
    [SerializeField]
    private CardsRowSingleCardShowdownAnimation[] singleCardShowdownAnimations;

    [SerializeField]
    private CardFlipAnimation[] cardFlipAnimations;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            PlayAllCardsShowdownAnimation().Forget();
        }
    }

    public async UniTask PlayAllCardsShowdownAnimation(float delay = 0.5f)
    {
        var tasks = new List<UniTask>(singleCardShowdownAnimations.Length);
        foreach (var animation in singleCardShowdownAnimations)
        {
            tasks.Add(animation.Play());
            await UniTask.Delay(TimeSpan.FromSeconds(delay));
        }
        await UniTask.WhenAll(tasks);
    }

    public void ResetAllCardsShowdownAnimation()
    {
        foreach (var animation in singleCardShowdownAnimations)
        {
            animation.ResetAnimation();
        }
    }

    public async UniTask PlaySingleCardShowdownAnimation(int position)
    {  
        if (position < 0 || position >= singleCardShowdownAnimations.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(position), "Position is out of range");
        }
        await singleCardShowdownAnimations[position].Play();
    }

    public async UniTask PlaySingleCardFlipAnimation(int position)
    {
        
    }

    public async UniTask PlayAllCardsFlipAnimation(float delay = 0.5f)
    {
        var tasks = new List<UniTask>(cardFlipAnimations.Length);
        foreach (var animation in cardFlipAnimations)
        {
            tasks.Add(animation.Play());
            await UniTask.Delay(TimeSpan.FromSeconds(delay));
        }
        await UniTask.WhenAll(tasks);
    }

    
}