using UnityEngine;

[CreateAssetMenu(fileName = "NewCardData", menuName = "Create/CardData")]
public class CardData : ScriptableObject
{
    public string id;
    public int level;
    public int points;
    public SkillType skill;
    public SkillCostMap cost;
}
