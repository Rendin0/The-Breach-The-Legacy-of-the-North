
public class CmdDeleteCreature : ICommand
{
    public int EntityId { get; }

    public CmdDeleteCreature(int entityId)
    {
        EntityId = entityId;
    }
}

