
public class SESpeedChange : IStatusEffect
{
    private readonly float _power;
    private readonly bool _isPercent;
    public SESpeedChange(float power, bool isPercent = false)
    {
        _power = power;
        _isPercent = isPercent;
    }

    public void Apply(CreatureStatsViewModel stats)
    {
        if (_isPercent)
            stats.Speed.OnNext(stats.Speed.Value * _power);
        else
            stats.Speed.OnNext(stats.Speed.Value + _power);

    }
}
