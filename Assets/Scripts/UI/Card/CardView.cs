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
    
    public void Setup(CardModel model)
    {
        if (model == null)
        {
            gameObject.SetActive(false);
            return;
        }

        gameObject.SetActive(true);
        costElement.Setup(model.Cost);
        pointsText.text = model.Points.ToString();
        SetupSkillImage(model.Skill);
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