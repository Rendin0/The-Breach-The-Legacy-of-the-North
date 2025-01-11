using UnityEngine;

public class Fireball : SpellBase
{
    [SerializeField] private ProjectileBase firballProjectile;



    public override void Use(GameObject caster, Vector2 direction)
    {
        var fireball = Instantiate(firballProjectile, ParticleContainer.instance.transform);
        var dirNormalized = (direction - (Vector2)caster.transform.position).normalized;
        fireball.Init(dirNormalized);

        // Спавн фаербола за пределами кастера
        fireball.transform.position = (Vector2)caster.transform.position
            + (Vector2)caster.transform.localScale * caster.GetComponent<BoxCollider2D>().size / 2 * dirNormalized
            + dirNormalized;
    }

    


}
