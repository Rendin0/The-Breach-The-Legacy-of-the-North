using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreatureBase : MonoBehaviour, IDamageable
{
    [SerializeField] protected float maxHealth = 100f;
    [SerializeField] protected float health = 100f;
    [SerializeField] protected ItemBase hand;

    [SerializeField] private GameObject damagePopUpPrefab;

    private Slider healthSlider;

    public virtual void Damage(float damage)
    {
        health -= damage;
        GenerateDamagePopUp(damage, Color.red);
        UpdateHealth();

        if (health <= 0)
            Destroy(gameObject);
    }
    public virtual void Heal(float heal)
    {
        health += heal;
        if (health > maxHealth)
            health = maxHealth;

        GenerateDamagePopUp(heal, Color.green);
        UpdateHealth();
    }

    private void GenerateDamagePopUp(float damage, Color color)
    {
        var popUp = Instantiate(damagePopUpPrefab, ParticleContainer.instance.transform);
        popUp.transform.position = transform.position + new Vector3(Random.Range(-.8f, .8f), 0.5f, -2);
        var text = popUp.GetComponentInChildren<TextMeshProUGUI>();
        text.text = damage.ToString();
        text.color = color;

        Destroy(popUp, 1f);
    }

    private void UpdateHealth()
    {
        if (healthSlider == null)
            healthSlider = GetComponentInChildren<Slider>();

        if (healthSlider != null)
        {
            healthSlider.value = health / maxHealth;
        }
    }
}
