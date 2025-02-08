public class CmdKillCreature : ICommand
{
    public readonly int Id;

    public CmdKillCreature(int id)
    {
        this.Id = id;
    }
}