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

    [SerializeField] private GameObject frontSide;
    [SerializeField] private GameObject backSide;   

    [SerializeField] private GameObject container;

    public Action OnCardClicked;

    public void Initialize()
    {
        SetVisible(false);
    }

    public void Setup(CardData data)
    {
        InitialCardState();
        costElement.Setup(data.Cost);
        pointsText.text = data.Points.ToString();
        SetupSkillImage(data.Skill);
    }

    public void SetVisible(bool value)
    {
        container.SetActive(value);
    }

    public void InitialCardState()
    {
        frontSide.SetActive(false);
        backSide.SetActive(true);
    }

    public void ShowFrontSide()
    {
        frontSide.SetActive(true);
        backSide.SetActive(false);
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