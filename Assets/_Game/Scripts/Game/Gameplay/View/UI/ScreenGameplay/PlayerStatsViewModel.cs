using R3;

public class PlayerStatsViewModel
{
    public ReactiveProperty<float> health;
    public ReactiveProperty<float> maxHealth;
    public ReactiveProperty<float> stamina;
    public ReactiveProperty<float> maxStamina;


    public PlayerStatsViewModel(CreatureViewModel creature)
    {
        health = creature.Stats.Health;
        maxHealth = creature.Stats.MaxHealth;
        stamina = creature.Stats.Stamina;
        maxStamina = creature.Stats.MaxStamina;
    }
}