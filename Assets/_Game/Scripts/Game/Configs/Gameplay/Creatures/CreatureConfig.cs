
using UnityEngine;

public enum Fractions
{
    Player,
    Undead
}


[CreateAssetMenu(fileName = "CreatureConfig_", menuName = "Game Config/Creatures/New Creature Config")]
public class CreatureConfig : ScriptableObject
{
    public string TypeId;
    public CreatureStatsConfig Stats;
    public Fractions Fraction;
}