using System.Collections.Generic;
using DefaultNamespace;
using Unity.VisualScripting;
using UnityEngine;

public class CardPoolRuntime
{
    private Dictionary<int, List<CardModel>> modelsPerLevel = new();

    public CardPoolRuntime(CardPool cardPool)
    {
        modelsPerLevel.Add(1, cardPool.GetCardModels(1));
        modelsPerLevel.Add(2, cardPool.GetCardModels(2));
        modelsPerLevel.Add(3, cardPool.GetCardModels(3));
    }

    public CardModel GetRandomCard(int level)
    {
        if (level < 0 || level > 3 || modelsPerLevel[level].Count == 0)
        {
            return null;
        }

        int index = Random.Range(0, modelsPerLevel[level].Count);
        var model = modelsPerLevel[level][index];
        modelsPerLevel[level].RemoveAt(index);
        return model;
    }
}