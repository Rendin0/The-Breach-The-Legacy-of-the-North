
public class SESlow : IStatusEffect
{
    private readonly float _slowPower;
    public SESlow(float slowPower)
    {
        _slowPower = slowPower;
    }

    public void Apply(CreatureStatsViewModel stats)
    {
        stats.Speed.OnNext(stats.Speed.Value * _slowPower);
    }
}
