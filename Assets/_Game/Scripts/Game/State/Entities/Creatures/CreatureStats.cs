
using System;

[Serializable]
public class CreatureStats
{
    public float Health;
    public float MaxHealth;
    public float Speed;
    public DamageData Damage;
    public bool Immortal;
    public float Defense;
    public float Resistance;
    public float AttackSpeed;
    public float Stamina;
    public float MaxStamina;
    public float DamageResistance;

    public CreatureStats(CreatureStatsConfig stats)
    {
        Health = stats.Health;
        MaxHealth = stats.MaxHealth;
        Speed = stats.Speed;
        Damage = stats.Damage;
        Immortal = stats.Immortal;
        Defense = stats.Defense;
        Resistance = stats.Resistance;
        AttackSpeed = stats.AttackSpeed;
        DamageResistance = stats.DamageResistance;
        Stamina = stats.Stamina;
        MaxStamina = stats.MaxStamina;
    }

}