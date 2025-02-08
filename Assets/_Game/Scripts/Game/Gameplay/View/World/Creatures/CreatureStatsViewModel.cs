using R3;
using System;

public class CreatureStatsViewModel
{
    public ReactiveProperty<float> Health { get; }
    public ReactiveProperty<float> MaxHealth { get; }
    public ReactiveProperty<float> Speed { get; }
    public ReactiveProperty<float> Damage { get; }

    public CreatureStatsViewModel(CreatureStatsProxy origin)
    {
        Health = origin.Health;
        MaxHealth = origin.MaxHealth;
        Speed = origin.Speed;
        Damage = origin.Damage;
    }

    public void CopyValuesTo(CreatureStatsViewModel other)
    {
        other.Health.OnNext(Health.Value);
        other.MaxHealth.OnNext(MaxHealth.Value);
        other.Speed.OnNext(Speed.Value);
        other.Damage.OnNext(Damage.Value);
    }

    public void CopyFrom(CreatureStatsProxy baseStats)
    {
        Health.OnNext(baseStats.Health.Value);
        MaxHealth.OnNext(baseStats.MaxHealth.Value);
        Speed.OnNext(baseStats.Speed.Value);
        Damage.OnNext(baseStats.Damage.Value);
    }
}