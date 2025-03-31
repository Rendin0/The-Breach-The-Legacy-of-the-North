
using System.Collections;
using System.Collections.Generic;
using ObservableCollections;
using R3;
using UnityEngine;

public abstract class AgentViewModel : CreatureViewModel
{
    public readonly List<IAbility> Abilities = new();
    public CreatureViewModel CurrentTarget { get; set; }
    public AgentTypes AgentType => creatureEntity.AgentType;
    public ObservableDictionary<CreatureViewModel, float> ThreatMap { get; } = new();

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
        GameEntryPoint.Coroutines.StartCoroutine(ThreatCoroutine(creature, threat));
    }

    private IEnumerator ThreatCoroutine(CreatureViewModel key, float threat)
    {
        yield return new WaitForSeconds(_rememberTime);

        for (int i = 0; i < 10; i++)
        {
            yield return new WaitForSeconds(1f);
            ThreatMap[key] -= threat * 0.1f;
        }

        if (ThreatMap[key] <= 0f)
            ThreatMap.Remove(key);
    }
}