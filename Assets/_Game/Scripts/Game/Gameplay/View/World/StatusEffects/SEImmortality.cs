
public class SEImmortality : IStatusEffect
{
    public void Apply(CreatureStatsViewModel stats)
    {
        stats.Immortal.OnNext(true);
    }
}