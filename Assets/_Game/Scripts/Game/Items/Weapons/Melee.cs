using UnityEngine;

public class Melee : ItemBase
{
    [SerializeField] private float damage;
    [SerializeField] private float attackRadius;

    [SerializeField] private LayerMask attackLayer;

    [SerializeField] private Attack attackPrefab;

    public override void Use(GameObject user, Vector2 direction)
    {
        Vector2 attackDirection = (direction - (Vector2)transform.position).normalized;
        var attackPos = (Vector2)transform.position + attackDirection * attackRadius;

        Collider2D[] hits = Physics2D.OverlapCircleAll(attackPos, attackRadius, attackLayer);

        CreateAttackObject(attackDirection);

        foreach (Collider2D enemy in hits)
        {
            var damagable = enemy.GetComponent<IDamageable>();
            damagable.Damage(damage);
        }
    }

    private void CreateAttackObject(Vector2 attackDirection)
    {
        var attack = Instantiate(attackPrefab, ParticleContainer.instance.transform);
        attack.transform.position = (Vector2)transform.position + attackDirection * attackRadius;
        attack.transform.localScale *= attackRadius;
    }
}
