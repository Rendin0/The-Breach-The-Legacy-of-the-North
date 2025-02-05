
public class Ability
{
    public readonly EventAbility Use;
    public readonly string Name;

    public Ability(AbilityConfig config)
    {
        Use = config.Use;
        Name = config.Name;
    }
}