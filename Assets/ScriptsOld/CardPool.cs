using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCardPool", menuName = "Create/CardPool")]
public class CardPool : ScriptableObject
{
    [SerializeField] private CardDataSO[] level1Cards;
    [SerializeField] private CardDataSO[] level2Cards;
    [SerializeField] private CardDataSO[] level3Cards;

    public List<CardData> GetCardModels(int level)
    {
        var levelCards = level switch
        {
            1 => level1Cards,
            2 => level2Cards,
            3 => level3Cards,
            _ => null,
        };

        return levelCards?.Select(c => new CardData(c)).ToList();
    }
}