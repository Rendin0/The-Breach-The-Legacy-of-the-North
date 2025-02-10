
using R3;

public class CreatureStatsProxy
{
    public CreatureStats Origin;

    public ReactiveProperty<float> Health { get; }
    public ReactiveProperty<float> MaxHealth { get; }
    public ReactiveProperty<float> Speed { get; }
    public ReactiveProperty<float> Damage { get; }
    public ReactiveProperty<bool> Immortal { get; }
    public ReactiveProperty<float> Defense { get; }
    public ReactiveProperty<float> AttackSpeed { get; }

    public CreatureStatsProxy(CreatureStats origin)
    {
        Origin = origin;

        Health = new ReactiveProperty<float>(origin.Health);
        MaxHealth = new ReactiveProperty<float>(origin.MaxHealth);
        Speed = new ReactiveProperty<float>(origin.Speed);
        Damage = new ReactiveProperty<float>(origin.Damage);
        Immortal = new ReactiveProperty<bool>(origin.Immortal);
        Defense = new ReactiveProperty<float>(origin.Defense);
        AttackSpeed = new ReactiveProperty<float>(origin.AttackSpeed);

        Health.Skip(1).Subscribe(value => origin.Health = value);
        MaxHealth.Skip(1).Subscribe(value => origin.MaxHealth = value);
        Speed.Skip(1).Subscribe(value => origin.Speed = value);
        Damage.Skip(1).Subscribe(value => origin.Damage = value);
        Immortal.Skip(1).Subscribe(value => origin.Immortal = value);
        Defense.Skip(1).Subscribe(value => origin.Defense = value);
        AttackSpeed.Skip(1).Subscribe(value => origin.AttackSpeed = value);
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
            AttackSpeed = AttackSpeed.Value,
        };
        var statsOrigin = new CreatureStats(tmpCfg);

        return new(statsOrigin);
    }
}