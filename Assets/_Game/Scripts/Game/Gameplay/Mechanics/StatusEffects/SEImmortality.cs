
public class SEImmortality : IStatusEffect
{
    public void Apply(CreatureViewModel creature)
    {
        creature.Stats.Immortal.OnNext(true);
    }
}