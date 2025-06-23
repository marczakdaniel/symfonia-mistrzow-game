using UnityEngine.Serialization;

[System.Serializable]
public class CardCost
{
    public SkillType skill;
    public int cost;
}

[System.Serializable]
public class SkillCostMap
{
    public int blue;
    public int green;
    public int red;
    public int brown;
    public int purple;
}