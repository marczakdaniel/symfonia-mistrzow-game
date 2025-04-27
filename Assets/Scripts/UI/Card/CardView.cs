using System;
using DefaultNamespace.ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardView : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private CardCostElement costElement;
    [SerializeField] private TextMeshProUGUI pointsText;
    [SerializeField] private Image skillImage;
    [SerializeField] private TokensImagesSO tokenImages;

    public Action OnCardClicked;

    public void Setup(CardData data)
    {
        if (data == null)
        {
            gameObject.SetActive(false);
            return;
        }

        gameObject.SetActive(true);
        costElement.Setup(data.Cost);
        pointsText.text = data.Points.ToString();
        SetupSkillImage(data.Skill);
    }
    
    private void SetupSkillImage(SkillType modelSkill)
    {
        skillImage.sprite = modelSkill switch
        {
            SkillType.Blue => tokenImages.GetTokenImages(TokenType.Blue).stackImage1,
            SkillType.Red => tokenImages.GetTokenImages(TokenType.Red).stackImage1,
            SkillType.Brown => tokenImages.GetTokenImages(TokenType.Brown).stackImage1,
            SkillType.Green => tokenImages.GetTokenImages(TokenType.Green).stackImage1,
            SkillType.Purple => tokenImages.GetTokenImages(TokenType.Purple).stackImage1,
            _ => null,
        };
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnCardClicked?.Invoke();
    }

    public void PlayRotateAnimation()
    {
        
    }
}