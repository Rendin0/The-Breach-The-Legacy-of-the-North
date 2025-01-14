using UnityEngine;

public interface IControllable
{


    public void Move(Vector2 direction);
    public void UseHand(Vector2 mousePosition);
    public void UseSpell(int index, Vector2 mousePosition);

}
