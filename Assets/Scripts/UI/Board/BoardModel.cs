using System.Collections.Generic;
using System.Linq;

public class BoardModel
{
    public CardsRowModel[] CardsRowModels;

    public BoardModel(List<CardModel> level1CardModels, List<CardModel> level2CardModels, 
        List<CardModel> level3CardModels)
    {
        CardsRowModels = new CardsRowModel[3];
        
        CardsRowModels[0] = new CardsRowModel(1, level1CardModels);
        CardsRowModels[1] = new CardsRowModel(2, level2CardModels);
        CardsRowModels[2] = new CardsRowModel(3, level3CardModels);
    }

    public CardsRowModel GetCardsRowModelForLevel(int level)
    {
        if (level < 1 || level > 3)
        {
            return null;
        }
        return CardsRowModels[level - 1];
    }
}