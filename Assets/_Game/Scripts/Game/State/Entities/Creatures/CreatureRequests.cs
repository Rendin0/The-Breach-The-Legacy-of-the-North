using R3;

public class CreatureRequests
{
    public readonly Subject<CreatureViewModel> DeleteRequest = new();
    public readonly Subject<CreatureViewModel> KillRequest = new();
    public readonly Subject<CreatureViewModel> OnCreatureClick = new();
}