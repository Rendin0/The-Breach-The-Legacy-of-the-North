using UnityEngine;

public abstract class EnemyBase : MonoBehaviour, IDamageable
{
    private float health = 100;

    [SerializeField] protected ItemBase hand;

    public void Damage(float damage)
    {
        health -= damage;

        if (health <= 0)
            Destroy(gameObject);
    }
}
