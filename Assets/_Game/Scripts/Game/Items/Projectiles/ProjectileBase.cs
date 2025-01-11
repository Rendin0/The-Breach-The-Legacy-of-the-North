using UnityEngine;

public abstract class ProjectileBase : MonoBehaviour
{
    [SerializeField] protected float damage = 15f;
    [SerializeField] protected float speed = 45f;
    [SerializeField] protected float lifetime = 3f;

    private Rigidbody2D rb;

    private Vector2 moveDirection;

    public virtual void Init(Vector2 direction)
    {
        rb = GetComponent<Rigidbody2D>();
        moveDirection = direction;
    }

    protected virtual void Update()
    {
        lifetime -= Time.deltaTime;
        if (lifetime <= 0)
            Destroy(gameObject);
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveDirection * speed * Time.fixedDeltaTime);
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        var obj = collision.gameObject.GetComponent<IDamageable>();

        if (obj != null)
        {
            obj.Damage(damage);
        }


        Destroy(gameObject);
    }
}
