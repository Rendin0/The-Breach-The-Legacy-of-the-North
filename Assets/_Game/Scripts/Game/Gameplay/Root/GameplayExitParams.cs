public class GameplayExitParams
{
    public MainMenuEnterParams ExitParams { get; }

    public GameplayExitParams(MainMenuEnterParams exitParams)
    {
        ExitParams = exitParams;
    }

}
