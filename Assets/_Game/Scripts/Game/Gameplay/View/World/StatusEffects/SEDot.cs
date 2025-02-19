
using System.Collections;
using UnityEngine;

public class SEDot : IStatusEffect
{
    private CreatureViewModel _owner;
    private readonly float _totalDamage;
    private readonly float _tickSpeed;
    private readonly float _duration;

    public SEDot(float totalDamage, float duration, float tickSpeed = 0.33f)
    {
        this._totalDamage = totalDamage;
        this._tickSpeed = tickSpeed;
        this._duration = duration;
    }

    public void Apply(CreatureViewModel creature)
    {
        _owner = creature;
        GameEntryPoint.Coroutines.StartCoroutine(Timer());
    }

    private IEnumerator Timer()
    {
        float totalTicks = _duration / _tickSpeed;
        float damagePerTick = _totalDamage / totalTicks;
        var damage = new DamageData() { MagicalData = damagePerTick };

        for (int i = 0; i < totalTicks; i++)
        {
            _owner.Damage(damage);
            yield return new WaitForSeconds(_tickSpeed);
        }

        _owner.RemoveStatusEffect(this);
    }
}