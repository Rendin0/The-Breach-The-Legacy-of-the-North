
public class SESpeedChange : IStatusEffect
{
    private readonly float _power;
    private readonly bool _isPercent;
    public SESpeedChange(float power, bool isPercent = false)
    {
        _power = power;
        _isPercent = isPercent;
    }

    public void Apply(CreatureViewModel creature)
    {
        if (_isPercent)
            creature.Stats.Speed.OnNext(creature.Stats.Speed.Value * _power);
        else
            creature.Stats.Speed.OnNext(creature.Stats.Speed.Value + _power);

    }
}
