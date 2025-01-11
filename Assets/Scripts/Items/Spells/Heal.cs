using UnityEngine;

public class Heal : SpellBase
{
    [SerializeField] private float healPower = 10f;

    public override void Use(GameObject caster, Vector2 direction)
    {
        if (CheckCaster(caster))
        {
            var creature = caster.GetComponent<CreatureBase>();
            creature.Heal(healPower);
        }
        else
        {
            Debug.Log("Wrong weapon type");
        }
    }
}
