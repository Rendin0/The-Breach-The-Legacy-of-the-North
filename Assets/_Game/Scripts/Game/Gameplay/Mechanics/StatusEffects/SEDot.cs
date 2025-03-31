
using System.Collections;
using UnityEngine;

public class SEDot : IStatusEffect
{
    private CreatureViewModel _owner;
    private readonly CreaturesSerivce _creaturesSerivce;
    private readonly CreatureViewModel _dealer;
    private readonly float _totalDamage;
    private readonly float _tickSpeed;
    private readonly float _duration;

    public SEDot(CreaturesSerivce creaturesSerivce, CreatureViewModel dealer, float totalDamage, float duration, float tickSpeed = 0.33f)
    {
        _dealer = dealer;
        _creaturesSerivce = creaturesSerivce;
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
            _creaturesSerivce.DamageCreature(_owner, _dealer, damage);
            yield return new WaitForSeconds(_tickSpeed);
        }

        _owner.RemoveStatusEffect(this);
    }
}