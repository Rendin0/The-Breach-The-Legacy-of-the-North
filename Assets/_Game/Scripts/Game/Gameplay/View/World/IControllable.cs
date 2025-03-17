
using UnityEngine;

public interface IControllable
{
    public void Move(Vector2 direction);
    public void UseAbility(int index, Vector2 position);
    public bool Attack(Vector2 position);
}
