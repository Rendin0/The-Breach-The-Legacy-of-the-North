using UnityEngine;

public class Player : CreatureBase, IControllable
{
    [SerializeField] private float speed = 15f;

    private Rigidbody2D rb;

    Vector2 _movement;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spellBook = new SpellBase[5];
    }
    public void UseHand(Vector2 mousePosition)
    {
        hand.Use(gameObject, mousePosition);
    }
    public void Move(Vector2 direction)
    {
        _movement = direction;
    }
    private void Update()
    {
        hand.transform.position = transform.position + new Vector3(0.5f, 0.5f, 0);
    }
    private void FixedUpdate()
    {
        Vector2 movement = rb.position + _movement.normalized * speed * Time.fixedDeltaTime;
        rb.MovePosition(movement);
    }

    public void UseSpell(int index, Vector2 mousePosition)
    {
        if (spellBook[index] != null)
        {
            spellBook[index].Use(gameObject, mousePosition);
        }
    }
}
