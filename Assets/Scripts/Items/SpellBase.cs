using UnityEngine;

public class SpellBase : MonoBehaviour, IUsable
{
    public WeaponType weaponRestriction;

    public virtual void Use(GameObject caster, Vector2 direction) { }
}
