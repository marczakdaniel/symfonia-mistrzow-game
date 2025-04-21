using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardView : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private CardCostElement costElement;
    [SerializeField] private TextMeshProUGUI pointsText;
    [SerializeField] private Image skillImage;

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
        skillImage.color = modelSkill switch
        {
            SkillType.Blue => Color.blue,
            SkillType.Red => Color.red,
            SkillType.Yellow => Color.yellow,
            SkillType.Green => Color.green,
            SkillType.Magenta => Color.magenta,
            _ => skillImage.color
        };
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnCardClicked?.Invoke();
    }
}