
public class SEDefenseChange : IStatusEffect
{
    private readonly float _amount;
    private readonly bool _isPercent;

    public SEDefenseChange(float amount, bool isPercent = false)
    {
        _amount = amount;
        _isPercent = isPercent;
    }

    public void Apply(CreatureStatsViewModel stats)
    {
        if (_isPercent)
            stats.Defense.OnNext(stats.Defense.Value * _amount);
        else
            stats.Defense.OnNext(stats.Defense.Value + _amount);
    }
}

