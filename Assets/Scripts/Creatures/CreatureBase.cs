using TMPro;
using UnityEngine;

public class CreatureBase : MonoBehaviour, IDamageable
{
    [SerializeField] protected float health = 100f;
    [SerializeField] protected ItemBase hand;

    [SerializeField] private GameObject damagePopUpPrefab;

    public virtual void Damage(float damage)
    {
        health -= damage;
        GenerateDamagePopUp(damage);

        if (health <= 0)
            Destroy(gameObject);
    }

    private void GenerateDamagePopUp(float damage)
    {
        var popUp = Instantiate(damagePopUpPrefab, ParticleContainer.instance.transform);
        popUp.transform.position = transform.position + new Vector3(Random.Range(-.8f, .8f), 0.5f, -2);
        var text = popUp.GetComponentInChildren<TextMeshProUGUI>();
        text.text = damage.ToString();

        Destroy(popUp, 1f);
    }
}
