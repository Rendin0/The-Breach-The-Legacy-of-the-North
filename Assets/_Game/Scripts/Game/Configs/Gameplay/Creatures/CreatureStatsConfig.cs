using System;
using UnityEngine;

//[CreateAssetMenu(fileName = "CreatureStatsConfig_", menuName = "Game Config/Creatures/New Creature Stats Config")]
[Serializable]
public class CreatureStatsConfig /*: ScriptableObject*/
{
    [Min(0f)] public float MaxHealth;
    [Min(0f)] public float Health;
    [Min(0f)] public float Speed;
    [Min(0f)] public float Damage;
}