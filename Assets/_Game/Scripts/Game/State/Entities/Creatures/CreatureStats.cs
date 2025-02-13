
using System;

[Serializable]
public class CreatureStats
{
    public float Health;
    public float MaxHealth;
    public float Speed;
    public float Damage;
    public bool Immortal;
    public float Defense;
    public float AttackSpeed;
    public float Stamina;
    public float MaxStamina;

    public CreatureStats(CreatureStatsConfig stats)
    {
        Health = stats.Health;
        MaxHealth = stats.MaxHealth;
        Speed = stats.Speed;
        Damage = stats.Damage;
        Immortal = stats.Immortal;
        Defense = stats.Defense;
        AttackSpeed = stats.AttackSpeed;
        Stamina = stats.Stamina;
        MaxStamina = stats.MaxStamina;
    }

}