using UnityEngine;

public class CardCostElement : MonoBehaviour
{
    [SerializeField] private CardSingleCostElement blueCostElement;
    [SerializeField] private CardSingleCostElement redCostElement;
    [SerializeField] private CardSingleCostElement yellowCostElement;
    [SerializeField] private CardSingleCostElement greenCostElement;
    [SerializeField] private CardSingleCostElement magentaCostElement;
    
    public void Setup(SkillCostMap costMap)
    {
        blueCostElement.Setup(SkillType.Blue, costMap.blue);
        redCostElement.Setup(SkillType.Red, costMap.red);
        yellowCostElement.Setup(SkillType.Yellow, costMap.yellow);
        greenCostElement.Setup(SkillType.Green, costMap.green);
        magentaCostElement.Setup(SkillType.Magenta, costMap.magenta);
    }
}