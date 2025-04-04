
using ObservableCollections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AgentViewModel : CreatureViewModel
{
    public readonly List<IAbility> Attacks = new();
    public readonly List<IAbility> Heals = new();
    public CreatureViewModel CurrentTarget { get; set; }

    public AgentTypes AgentType => creatureEntity.AgentType;

    public ObservableDictionary<CreatureViewModel, float> ThreatMap { get; } = new();
    private readonly Dictionary<CreatureViewModel, Coroutine> _threatCoroutines = new();


    private const float _rememberTime = 5f;

    public AgentViewModel(CreatureEntityProxy creatureEntity)
        : base(creatureEntity)
    {
    }

    public override void Dispose()
    {
        foreach (var coroutine in _threatCoroutines)
            GameEntryPoint.Coroutines.StopCoroutine(coroutine.Value);
    }

    #region Abilities
    public void UseAttack(string name, Vector2 position)
    {
        var index = Attacks.FindIndex(a => a.ElementName == name);

        if (index == -1)
            return;

        UseAttack(index, position);
    }
    public void UseAttack(int index, Vector2 position)
    {
        if (index >= Attacks.Count)
            return;

        if (Attacks[index].Use(this, position))
        {
            // Перезарядка всем остальным на одну секунду, чтобы не было спама
            foreach (var ability in Attacks)
                ability.SetCooldown(1f);
        }
    }

    public void UseHeal(string name)
    {
        var index = Heals.FindIndex(a => a.ElementName == name);

        if (index == -1)
            return;

        UseHeal(index);
    }
    public void UseHeal(int index)
    {
        if (index >= Heals.Count)
            return;

        if (Heals[index].Use(this, Vector2.zero))
        {
            // Перезарядка всем остальным на одну секунду, чтобы не было спама
            foreach (var ability in Heals)
                ability.SetCooldown(1f);
        }
    }
    #endregion

    #region Threat
    public void StartRemoveThreatCoroutine(CreatureViewModel creature)
    {
        if (!ThreatMap.ContainsKey(creature))
            return;

        _threatCoroutines.Add(creature, GameEntryPoint.Coroutines.StartCoroutine(ThreatCoroutine(creature)));
    }

    public void AbortRemoveThreatCoroutine(CreatureViewModel creature)
    {
        if (!_threatCoroutines.ContainsKey(creature))
            return;

        GameEntryPoint.Coroutines.StopCoroutine(_threatCoroutines[creature]);
        _threatCoroutines.Remove(creature);
    }

    private IEnumerator ThreatCoroutine(CreatureViewModel key)
    {
        yield return new WaitForSeconds(_rememberTime);

        while (ThreatMap.TryGetValue(key, out var threat) && threat >= 0f)
        {
            yield return new WaitForSeconds(1f);

            if (!ThreatMap.ContainsKey(key))
                break;

            ThreatMap[key] -= 1f;
        }

        _threatCoroutines.Remove(key);
    }
    #endregion
}