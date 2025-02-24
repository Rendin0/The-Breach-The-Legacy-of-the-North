
public class SEPhysicalDamageChange : IStatusEffect
{
    private readonly float _amount;
    private readonly bool _isPercent;

    public SEPhysicalDamageChange(float upgrade, bool isPercent = false)
    {
        this._amount = upgrade;
        _isPercent = isPercent;
    }

    public void Apply(CreatureViewModel creature)
    {
        if (_isPercent)
            creature.Stats.Damage.Physical.OnNext(creature.Stats.Damage.Physical.Value * _amount);
        else
            creature.Stats.Damage.Physical.OnNext(creature.Stats.Damage.Physical.Value + _amount);
    }
}
