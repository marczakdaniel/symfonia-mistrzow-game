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
            SkillType.Blue => tokenImages.GetTokenCardImage(TokenType.Blue),
            SkillType.Red => tokenImages.GetTokenCardImage(TokenType.Red),
            SkillType.Brown => tokenImages.GetTokenCardImage(TokenType.Brown),
            SkillType.Green => tokenImages.GetTokenCardImage(TokenType.Green),
            SkillType.Purple => tokenImages.GetTokenCardImage(TokenType.Purple),
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