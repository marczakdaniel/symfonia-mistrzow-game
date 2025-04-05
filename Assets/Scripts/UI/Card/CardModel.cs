public class CardModel
{
    public string Id { get; private set; }
    public int Level { get; private set; }
    public int Points { get; private set; }
    public SkillType Skill { get; private set; }
    public SkillCostMap Cost { get; private set; }
    
    public CardModel(CardData cardData)
    {
        Id = cardData.id;
        Level = cardData.level;
        Points = cardData.points;
        Skill = cardData.skill;
        Cost = cardData.cost;
    }
}
