using UnityEngine;

public class Heal : SpellBase
{
    [SerializeField] private float healPower = 10f;

    public override void Use(GameObject caster, Vector2 direction)
    {
        var creature = caster.GetComponent<CreatureBase>();

        if (creature != null)
        {
            if (creature.hand.weaponType == weaponRestriction)
            {
                creature.Heal(healPower);
            }
            else
            {
                Debug.Log("Wrong weapon type");
            }
        }

    }
}
