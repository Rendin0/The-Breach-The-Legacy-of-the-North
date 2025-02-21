
using R3;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatsBinder : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Slider staminaSlider;

    private CompositeDisposable _subs = new();

    private void OnDestroy()
    {
        _subs.Dispose();
    }

    public void Bind(PlayerStatsViewModel origin)
    {
        origin.health.Subscribe(h => OnHealthChanged(h, origin.maxHealth.Value));
        origin.maxHealth.Subscribe(mh => OnHealthChanged(origin.health.Value, mh));

        origin.stamina.Subscribe(s => OnStaminaChanged(s, origin.maxStamina.Value));
        origin.maxStamina.Subscribe(ms => OnStaminaChanged(origin.stamina.Value, ms));
    }

    private void OnHealthChanged(float health, float maxHealth)
    {
        healthSlider.value = health / maxHealth;
    }

    private void OnStaminaChanged(float stamina, float maxStamina)
    {
        staminaSlider.value = stamina / maxStamina;
    }
}

