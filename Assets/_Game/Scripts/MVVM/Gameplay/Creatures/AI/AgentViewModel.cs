
using ObservableCollections;
using R3;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AgentViewModel : CreatureViewModel
{
    public readonly List<IAbility> Abilities = new();
    public CreatureViewModel CurrentTarget { get; set; }

    public AgentTypes AgentType => creatureEntity.AgentType;

    public ObservableDictionary<CreatureViewModel, float> ThreatMap { get; } = new();
    private readonly ObservableDictionary<CreatureViewModel, Coroutine> _threatCoroutines = new();


    private const float _rememberTime = 5f;

    public AgentViewModel(CreatureEntityProxy creatureEntity)
        : base(creatureEntity)
    {
        ThreatMap.ObserveAdd().Subscribe(pair => ThreatAdded(pair.Value.Key, pair.Value.Value));
    }

    public void UseAbility(int index, Vector2 position)
    {
        if (index >= Abilities.Count)
            return;

        if (Abilities[index].Use(this, position))
        {
            // Перезарядка всем остальным на одну секунду, чтобы не было спама
            foreach (var ability in Abilities)
                ability.SetCooldown(1f);
        }
    }

    private void ThreatAdded(CreatureViewModel creature, float threat)
    {
        if (_threatCoroutines.TryGetValue(creature, out var coroutine))
            GameEntryPoint.Coroutines.StopCoroutine(coroutine);

        _threatCoroutines[creature] = GameEntryPoint.Coroutines.StartCoroutine(ThreatCoroutine(creature));
    }

    private IEnumerator ThreatCoroutine(CreatureViewModel key)
    {
        yield return new WaitForSeconds(_rememberTime);

        for (int i = 0; i < 10; i++)
        {
            yield return new WaitForSeconds(1f);

            if (!ThreatMap.ContainsKey(key))
                yield break;

            ThreatMap[key] -= ThreatMap[key] * 0.1f;
        }

        _threatCoroutines.Remove(key);
    }

    public override void Dispose()
    {
        foreach (var coroutine in _threatCoroutines)
            GameEntryPoint.Coroutines.StopCoroutine(coroutine.Value);

    }
}