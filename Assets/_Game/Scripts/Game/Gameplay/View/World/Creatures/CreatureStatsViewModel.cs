using R3;
using System;

public class CreatureStatsViewModel
{
    public ReactiveProperty<float> Health { get; }
    public ReactiveProperty<float> MaxHealth { get; }
    public ReactiveProperty<float> Speed { get; }
    public ReactiveProperty<float> Damage { get; }
    public ReactiveProperty<bool> Immortal { get; }
    public ReactiveProperty<float> Defense { get; }
    public ReactiveProperty<float> AttackSpeed { get; }

    public CreatureStatsViewModel(CreatureStatsProxy origin)
    {
        Health = origin.Health;
        MaxHealth = origin.MaxHealth;
        Speed = origin.Speed;
        Damage = origin.Damage;
        Immortal = origin.Immortal;
        Defense = origin.Defense;
        AttackSpeed = origin.AttackSpeed;
    }

    public void CopyValuesTo(CreatureStatsViewModel other)
    {
        other.Health.OnNext(Health.Value);
        other.MaxHealth.OnNext(MaxHealth.Value);
        other.Speed.OnNext(Speed.Value);
        other.Damage.OnNext(Damage.Value);
        other.Immortal.OnNext(Immortal.Value);
        other.Defense.OnNext(Defense.Value);
        other.AttackSpeed.OnNext(AttackSpeed.Value);
    }

    public void CopyFrom(CreatureStatsProxy baseStats)
    {
        Health.OnNext(baseStats.Health.Value);
        MaxHealth.OnNext(baseStats.MaxHealth.Value);
        Speed.OnNext(baseStats.Speed.Value);
        Damage.OnNext(baseStats.Damage.Value);
        Immortal.OnNext(baseStats.Immortal.Value);
        Defense.OnNext(baseStats.Defense.Value);
        AttackSpeed.OnNext(baseStats.AttackSpeed.Value);
    }
}