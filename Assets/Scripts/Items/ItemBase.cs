using UnityEngine;

public abstract class ItemBase : MonoBehaviour, IUsable
{
    public virtual void Use(Vector2 direction) { }
}
