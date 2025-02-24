
public class SEDefenseChange : IStatusEffect
{
    private readonly float _amount;
    private readonly bool _isPercent;

    public SEDefenseChange(float amount, bool isPercent = false)
    {
        _amount = amount;
        _isPercent = isPercent;
    }

    public void Apply(CreatureViewModel creature)
    {
        if (_isPercent)
            creature.Stats.Defense.OnNext(creature.Stats.Defense.Value * _amount);
        else
            creature.Stats.Defense.OnNext(creature.Stats.Defense.Value + _amount);
    }
}

