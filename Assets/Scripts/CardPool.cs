using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCardPool", menuName = "Create/CardPool")]
public class CardPool : ScriptableObject
{
    [SerializeField] private CardData[] level1Cards;
    [SerializeField] private CardData[] level2Cards;
    [SerializeField] private CardData[] level3Cards;

    public List<CardModel> GetCardModels(int level)
    {
        var levelCards = level switch
        {
            1 => level1Cards,
            2 => level2Cards,
            3 => level3Cards,
            _ => null,
        };

        return levelCards?.Select(c => new CardModel(c)).ToList();
    }
}