
public class CmdCreateInventory : ICommand
{
    public readonly int OwnerId;
    public readonly int Size;

    public CmdCreateInventory(int ownerId, int size)
    {
        OwnerId = ownerId;
        Size = size;
    }
}