
public class CmdSortStorage : ICommand
{
    public readonly StorageViewModel Storage;

    public CmdSortStorage(StorageViewModel storage)
    {
        Storage = storage;
    }
}