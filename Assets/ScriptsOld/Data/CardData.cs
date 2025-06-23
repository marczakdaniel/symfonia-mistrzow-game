public class CardData
{
    public string Id { get; private set; }
    public int Level { get; private set; }
    public int Points { get; private set; }
    public SkillType Skill { get; private set; }
    public SkillCostMap Cost { get; private set; }
    
    public CardData(CardDataSO cardDataSo)
    {
        Id = cardDataSo.id;
        Level = cardDataSo.level;
        Points = cardDataSo.points;
        Skill = cardDataSo.skill;
        Cost = cardDataSo.cost;
    }
}
