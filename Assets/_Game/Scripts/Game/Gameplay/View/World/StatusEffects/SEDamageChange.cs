
public class SEDamageChange : IStatusEffect
{
    private readonly float _amount;
    private readonly bool _isPercent;

    public SEDamageChange(float upgrade, bool isPercent = false)
    {
        this._amount = upgrade;
        _isPercent = isPercent;
    }

    public void Apply(CreatureStatsViewModel stats)
    {
        if (_isPercent)
            stats.Damage.OnNext(stats.Damage.Value * _amount);
        else
            stats.Damage.OnNext(stats.Damage.Value + _amount);
    }
}
