
using R3;

public class CreatureStatsProxy
{
    public CreatureStats Origin;

    public ReactiveProperty<float> Health { get; }
    public ReactiveProperty<float> MaxHealth { get; }
    public ReactiveProperty<float> Speed { get; }
    public ReactiveProperty<float> Damage { get; }

    public CreatureStatsProxy(CreatureStats origin)
    {
        Origin = origin;

        Health = new ReactiveProperty<float>(origin.Health);
        MaxHealth = new ReactiveProperty<float>(origin.MaxHealth);
        Speed = new ReactiveProperty<float>(origin.Speed);
        Damage = new ReactiveProperty<float>(origin.Damage);

        Health.Skip(1).Subscribe(value => origin.Health = value);
        MaxHealth.Skip(1).Subscribe(value => origin.MaxHealth = value);
        Speed.Skip(1).Subscribe(value => origin.Speed = value);
        Damage.Skip(1).Subscribe(value => origin.Damage = value);
    }

    public CreatureStatsProxy Copy()
    {
        var tmpCfg = new CreatureStatsConfig()
        {
            Damage = Damage.Value,
            Health = Health.Value,
            MaxHealth = MaxHealth.Value,
            Speed = Speed.Value,
        };
        var statsOrigin = new CreatureStats(tmpCfg);

        return new(statsOrigin);
    }
}