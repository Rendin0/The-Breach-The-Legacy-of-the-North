public class MainMenuExitParams
{
    public SceneEnterParams ExitParams { get; }

    public MainMenuExitParams(SceneEnterParams exitParams)
    {
        ExitParams = exitParams;
    }
}
