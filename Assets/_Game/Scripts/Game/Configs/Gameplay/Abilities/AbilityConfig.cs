using System;
using UltEvents;
using UnityEngine;

[CreateAssetMenu(fileName = "AbilityConfig_", menuName = "Game Config/Abilities/New Ability Config")]
public class AbilityConfig : ScriptableObject
{
    public string Name;
    [TextArea] public string Description;
    public float CooldownTime;

    public EventAbility Use;
    public EventAbilityRequirement Requirement;
}

[Serializable]
public class EventAbility : UltEvent<CreatureViewModel, Vector2> { }

[Serializable]
public class EventAbilityRequirement : UltEvent<CreatureViewModel, BoolWrapper> { }
