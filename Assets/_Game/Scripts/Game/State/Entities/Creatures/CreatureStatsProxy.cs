
using R3;

public class CreatureStatsProxy
{
    public CreatureStats Origin;

    public ReactiveProperty<float> Health { get; }
    public ReactiveProperty<float> MaxHealth { get; }
    public ReactiveProperty<float> Speed { get; }
    public Damage Damage { get; }
    public ReactiveProperty<bool> Immortal { get; }
    public ReactiveProperty<float> Defense { get; }
    public ReactiveProperty<float> Resistance { get; }
    public ReactiveProperty<float> AttackSpeed { get; }
    public ReactiveProperty<float> DamageResistance { get; }
    public ReactiveProperty<float> Stamina { get; }
    public ReactiveProperty<float> MaxStamina { get; }

    public CreatureStatsProxy(CreatureStats origin)
    {
        Origin = origin;

        Health = new(origin.Health);
        MaxHealth = new(origin.MaxHealth);
        Speed = new(origin.Speed);
        Damage = new (origin.Damage);
        Immortal = new(origin.Immortal);
        Defense = new(origin.Defense);
        Resistance = new(origin.Resistance);
        AttackSpeed = new(origin.AttackSpeed);
        DamageResistance = new(origin.DamageResistance);
        Stamina = new(origin.Stamina);
        MaxStamina = new(origin.MaxStamina);

        Health.Skip(1).Subscribe(value => origin.Health = value);
        MaxHealth.Skip(1).Subscribe(value => origin.MaxHealth = value);
        Speed.Skip(1).Subscribe(value => origin.Speed = value);
        Immortal.Skip(1).Subscribe(value => origin.Immortal = value);
        Defense.Skip(1).Subscribe(value => origin.Defense = value);
        Resistance.Skip(1).Subscribe(value => origin.Resistance = value);
        AttackSpeed.Skip(1).Subscribe(value => origin.AttackSpeed = value);
        DamageResistance.Skip(1).Subscribe(value => origin.DamageResistance = value);
        Stamina.Skip(1).Subscribe(value => origin.Stamina = value);
        MaxStamina.Skip(1).Subscribe(value => origin.MaxStamina = value);
    }

    public CreatureStatsProxy Copy()
    {
        var tmpCfg = new CreatureStatsConfig()
        {
            Damage = Damage.Value,
            Health = Health.Value,
            MaxHealth = MaxHealth.Value,
            Speed = Speed.Value,
            Immortal = Immortal.Value,
            Defense = Defense.Value,
            Resistance = Resistance.Value,
            AttackSpeed = AttackSpeed.Value,
            DamageResistance = DamageResistance.Value,
            Stamina = Stamina.Value,
            MaxStamina = MaxStamina.Value,
        };
        var statsOrigin = new CreatureStats(tmpCfg);

        return new(statsOrigin);
    }
}