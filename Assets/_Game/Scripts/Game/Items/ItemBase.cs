using UnityEngine;

public enum WeaponType
{
    Melee,
    Magical
}

public abstract class ItemBase : MonoBehaviour, IUsable
{
    public WeaponType weaponType;

    public virtual void Use(GameObject user, Vector2 direction) { }
}
