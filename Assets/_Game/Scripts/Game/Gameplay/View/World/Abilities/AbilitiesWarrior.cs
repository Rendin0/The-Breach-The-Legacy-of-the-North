

using UnityEngine;
using UnityEngine.Events;

public static class AbilitiesWarrior
{
    public static void Slash(CreatureViewModel caster, Vector2 mousePosition)
    {
        Debug.Log($"{caster} + {mousePosition}");
    }


}