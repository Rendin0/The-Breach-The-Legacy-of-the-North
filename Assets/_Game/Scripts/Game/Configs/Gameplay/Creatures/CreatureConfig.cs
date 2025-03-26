
using UnityEngine;


[CreateAssetMenu(fileName = "CreatureConfig_", menuName = "Game Config/Creatures/New Creature Config")]
public class CreatureConfig : ScriptableObject
{
    public string TypeId;
    public CreatureStatsConfig Stats;
    public Factions Faction;
    public AgentTypes AgentType;
}