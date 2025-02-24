using System;
using UnityEngine;

//[CreateAssetMenu(fileName = "CreatureStatsConfig_", menuName = "Game Config/Creatures/New Creature Stats Config")]
[Serializable]
public class CreatureStatsConfig /*: ScriptableObject*/
{
    [Min(0f)] public float MaxHealth;
    [Min(1f)] public float Health;
    [Min(0f)] public float Speed;
    public DamageData Damage;
    public bool Immortal;
    public float Resistance;
    public float Defense;
    [Min(1f)] public float AttackSpeed;
    public float DamageResistance;
    public float Stamina;
    [Min(1f)] public float MaxStamina;

}