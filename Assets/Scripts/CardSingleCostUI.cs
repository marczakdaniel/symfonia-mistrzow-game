using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardSingleCostUI : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI costText;

    public void Setup(SkillType skill, int cost)
    {
        if (cost == 0)
        {
            gameObject.SetActive(false);
            return;
        }

        gameObject.SetActive(true);
        SetupImage(skill);
        SetupCostText(cost);
    }

    private void SetupCostText(int cost)
    {
        costText.text = cost.ToString();
    }

    private void SetupImage(SkillType skill)
    {
        image.color = skill switch
        {
            SkillType.Blue => Color.blue,
            SkillType.Red => Color.red,
            SkillType.Yellow => Color.yellow,
            SkillType.Green => Color.green,
            SkillType.Magenta => Color.magenta,
            _ => image.color
        };
    }
}
