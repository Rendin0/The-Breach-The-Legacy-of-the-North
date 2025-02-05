using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AbilitiesConfig", menuName = "Game Config/New Abilities Config")]
public class AbilitiesConfig : ScriptableObject
{
    public List<AbilityConfig> Abilities;
}
