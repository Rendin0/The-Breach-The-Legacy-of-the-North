using UnityEngine;

public class EnemySkeleton : EnemyBase
{
    [SerializeField] private float attackTimer = 2f;
    private float timer;

    private GameObject player;

    private void Start()
    {
        timer = attackTimer;
        player = GameObject.FindWithTag("Player");
    }

    private void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            if (player != null)
                Attack(player.transform.position);
            timer = attackTimer;
        }

    }

    private void Attack(Vector2 direction)
    {
        hand.Use(gameObject, direction);
    }
}
