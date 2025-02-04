
public class CmdDeleteInventory : ICommand
{
    public readonly int OwnerId;

    public CmdDeleteInventory(int ownerId)
    {
        OwnerId = ownerId;
    }
}