using System;
using UltEvents;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "AbilityConfig_", menuName = "Game Config/Abilities/New Ability Config")]
public class AbilityConfig : ScriptableObject
{
    public string Name;
    public float CooldownTime;

    public EventAbility Use;
}

[Serializable]
public class EventAbility : UltEvent<CreatureViewModel, Vector2> { }