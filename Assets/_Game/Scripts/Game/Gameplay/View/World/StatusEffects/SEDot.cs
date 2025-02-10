
using System.Collections;
using UnityEngine;

public class SEDot : IStatusEffect
{
    private readonly CreatureViewModel _owner;
    private readonly float _totalDamage;
    private readonly float _tickSpeed;
    private readonly float _duration;

    public SEDot(CreatureViewModel owner, float totalDamage, float duration, float tickSpeed = 0.33f)
    {
        this._owner = owner;
        this._totalDamage = totalDamage;
        this._tickSpeed = tickSpeed;
        this._duration = duration;
    }

    public void Apply(CreatureStatsViewModel stats)
    {
        GameEntryPoint.Coroutines.StartCoroutine(Timer());
    }

    private IEnumerator Timer()
    {
        float totalTicks = _duration / _tickSpeed;
        float damagePerTick = _totalDamage / totalTicks;
        
        for (int i = 0; i < totalTicks; i++)
        {
            _owner.Damage(damagePerTick);
            yield return new WaitForSeconds(_tickSpeed);
        }

        _owner.RemoveStatusEffect(this);
    }
}