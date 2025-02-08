
using System;

[Serializable]
public class CreatureStats
{
    public float Health;
    public float MaxHealth;
    public float Speed;
    public float Damage;

    public CreatureStats(CreatureStatsConfig stats)
    {
        Health = stats.Health;
        MaxHealth = stats.MaxHealth;
        Speed = stats.Speed;
        Damage = stats.Damage;
    }
}