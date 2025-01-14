
using R3;

public interface IGameStateProvider
{
    public GameStateProxy GameState { get; }
    public SettingsStateProxy SettingsState { get; }

    public Observable<GameStateProxy> LoadGameState();
    public Observable<bool> SaveGameState();
    public Observable<bool> ResetGameState();

    public Observable<SettingsStateProxy> LoadSettingsState();
    public Observable<bool> SaveSettingsState();
    public Observable<bool> ResetSettingsState();

}