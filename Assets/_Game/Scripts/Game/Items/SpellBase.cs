using UnityEngine;

public class SpellBase : MonoBehaviour, IUsable
{
    public WeaponType weaponRestriction;

    public virtual void Use(GameObject caster, Vector2 direction) { }

    protected bool CheckCaster(GameObject caster)
    {
        var creature = caster.GetComponent<CreatureBase>();

        return creature != null && creature.hand.weaponType == weaponRestriction; 
    }
}
