using System;
using UnityEngine;

public class CardsRowModel
{
    public Action ModelUpdated;
    
    public int Level { get; private set; }

    public int SlotCount = 4;

    public CardsRowModel(int level)
    {
        Level = level;
    }
}