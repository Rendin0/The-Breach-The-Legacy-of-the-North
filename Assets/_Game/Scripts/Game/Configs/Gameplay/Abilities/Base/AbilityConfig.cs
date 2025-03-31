using System;
using UltEvents;
using UnityEngine;

public abstract class AbilityConfig<T> : ScriptableObject where T : CreatureViewModel
{
    public string Name;
    [TextArea] public string Description;
    public float CooldownTime;

    public EventAbility Use;
    public EventAbilityRequirement Requirement;


    [Serializable]
    public class EventAbility : UltEvent<T, Vector2> { }

    [Serializable]
    public class EventAbilityRequirement : UltEvent<T, BoolWrapper> { }
}

