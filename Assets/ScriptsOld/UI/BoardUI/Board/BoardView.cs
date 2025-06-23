using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class BoardView : MonoBehaviour
{
    [SerializeField] private CardsRowView[] rowViews;
    [SerializeField] private BoardAnimationController boardAnimationController;
    
    
    public CardsRowView GetCardsRowView(int index)
    {
        if (index < 0 || index >= rowViews.Length)
        {
            return null;
        }

        return rowViews[index];
    }

    public async UniTask PlayAllShowdownAnimation()
    {
        await boardAnimationController.PlayAllShowdownAnimation();
    }
    
    public void ResetAllCardsShowdownAnimation()
    {
         boardAnimationController.ResetAllCardsShowdownAnimation();
    }

    public async UniTask PlaySingleCardShowdownAnimation(int row, int position)
    {
        await boardAnimationController.PlaySingleCardShowdownAnimation(row, position);
    }

    public void ResetSingleCardShowdownAnimation(int row, int position)
    {
        boardAnimationController.ResetSingleCardShowdownAnimation(row, position);
    }

    public async UniTask PlayAllFlipAnimation() 
    {
        await boardAnimationController.PlayAllFlipAnimation();
    }

    public async UniTask PlaySingleCardFlipAnimation(int row, int position)
    {
        await boardAnimationController.PlaySingleCardFlipAnimation(row, position);
    }
}