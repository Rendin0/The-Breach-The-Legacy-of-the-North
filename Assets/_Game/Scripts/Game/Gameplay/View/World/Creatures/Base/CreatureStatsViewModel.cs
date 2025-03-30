using R3;

public class CreatureStatsViewModel
{
    public ReactiveProperty<float> Health { get; }
    public ReactiveProperty<float> MaxHealth { get; }
    public ReactiveProperty<float> Speed { get; }
    public Damage Damage { get; }
    public ReactiveProperty<bool> Immortal { get; }
    public ReactiveProperty<float> Defense { get; }
    public ReactiveProperty<float> Resistance { get; }
    public ReactiveProperty<float> AttackSpeed { get; }
    public ReactiveProperty<float> Stamina { get; }
    public ReactiveProperty<float> MaxStamina { get; }
    public ReactiveProperty<float> DamageResistance { get; }

    public CreatureStatsViewModel(CreatureStatsProxy origin)
    {
        Health = origin.Health;
        MaxHealth = origin.MaxHealth;
        Speed = origin.Speed;
        Damage = origin.Damage;
        Immortal = origin.Immortal;
        Defense = origin.Defense;
        Resistance = origin.Resistance;
        AttackSpeed = origin.AttackSpeed;
        DamageResistance = origin.DamageResistance;
        Stamina = origin.Stamina;
        MaxStamina = origin.MaxStamina;

    }

    public void CopyValuesTo(CreatureStatsViewModel other)
    {
        other.Health.OnNext(Health.Value);
        other.MaxHealth.OnNext(MaxHealth.Value);
        other.Speed.OnNext(Speed.Value);
        other.Damage.Magical.OnNext(Damage.Magical.Value);
        other.Damage.Physical.OnNext(Damage.Physical.Value);
        other.Immortal.OnNext(Immortal.Value);
        other.Defense.OnNext(Defense.Value);
        other.Resistance.OnNext(Resistance.Value);
        other.AttackSpeed.OnNext(AttackSpeed.Value);
        other.DamageResistance.OnNext(DamageResistance.Value);
        other.Stamina.OnNext(Stamina.Value);
        other.MaxStamina.OnNext(MaxStamina.Value);

    }

    public void CopyFrom(CreatureStatsProxy baseStats)
    {
        Health.OnNext(baseStats.Health.Value);
        MaxHealth.OnNext(baseStats.MaxHealth.Value);
        Speed.OnNext(baseStats.Speed.Value);
        Damage.Magical.OnNext(baseStats.Damage.Magical.Value);
        Damage.Physical.OnNext(baseStats.Damage.Physical.Value);
        Immortal.OnNext(baseStats.Immortal.Value);
        Defense.OnNext(baseStats.Defense.Value);
        Resistance.OnNext(baseStats.Resistance.Value);
        AttackSpeed.OnNext(baseStats.AttackSpeed.Value);
        DamageResistance.OnNext(baseStats.DamageResistance.Value);
        Stamina.OnNext(baseStats.Stamina.Value);
        MaxStamina.OnNext(baseStats.MaxStamina.Value);
    }
}