using UnityEngine;

public class CardCostElement : MonoBehaviour
{
    [SerializeField] private CardSingleCostElement blueCostElement;
    [SerializeField] private CardSingleCostElement redCostElement;
    [SerializeField] private CardSingleCostElement brownCostElement;
    [SerializeField] private CardSingleCostElement greenCostElement;
    [SerializeField] private CardSingleCostElement purpleCostElement;
    
    public void Setup(SkillCostMap costMap)
    {
        blueCostElement.Setup(TokenType.Blue, costMap.blue);
        redCostElement.Setup(TokenType.Red, costMap.red);
        brownCostElement.Setup(TokenType.Brown, costMap.brown);
        greenCostElement.Setup(TokenType.Green, costMap.green);
        purpleCostElement.Setup(TokenType.Purple, costMap.purple);
    }
}