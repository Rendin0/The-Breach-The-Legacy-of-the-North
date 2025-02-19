
using System.Collections;
using UnityEngine;

public class TemporaryStatusEffect : IStatusEffect
{
    private readonly IBuffable _owner;
    private readonly IStatusEffect _effect;
    private readonly float _duration;

    public TemporaryStatusEffect(IBuffable owner, IStatusEffect effect, float duration)
    {
        _owner = owner;
        _effect = effect;
        _duration = duration;
    }

    public void Apply(CreatureViewModel creature)
    {
        _effect.Apply(creature);
        GameEntryPoint.Coroutines.StartCoroutine(Timer());
    }

    private IEnumerator Timer()
    {
        yield return new WaitForSeconds(_duration);

        _owner.RemoveStatusEffect(this);
    }
}