using System.Collections.Generic;

public class WarriorViewModel : CreatureViewModel
{
    public List<CreatureViewModel> MarkedTargets { get; } = new();
    public bool InfiniteRage = false;

    public WarriorViewModel(CreatureEntityProxy creatureEntity) 
        : base(creatureEntity)
    {
    }

}