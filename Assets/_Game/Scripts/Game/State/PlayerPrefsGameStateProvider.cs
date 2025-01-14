
using R3;
using System;
using UnityEngine;

public class PlayerPrefsGameStateProvider : IGameStateProvider
{
    private const string GAME_STATE_KEY = nameof(GAME_STATE_KEY);
    private const string SETTINGS_STATE_KEY = nameof(SETTINGS_STATE_KEY);

    public GameStateProxy GameState { get; private set; }
    public SettingsStateProxy SettingsState { get; private set; }

    private GameState _gameStateOrigin;
    private SettingsState _settingsStateOrigin;

    public Observable<GameStateProxy> LoadGameState()
    {
        if (!PlayerPrefs.HasKey(GAME_STATE_KEY))
        {
            GameState = CreateGameStateFromSettings();

            SaveGameState();
        }
        else
        {
            var json = PlayerPrefs.GetString(GAME_STATE_KEY);
            _gameStateOrigin = JsonUtility.FromJson<GameState>(json);
            GameState = new GameStateProxy(_gameStateOrigin);
        }

        return Observable.Return(GameState);
    }
    public Observable<bool> SaveGameState()
    {
        var json = JsonUtility.ToJson(_gameStateOrigin, true);
        PlayerPrefs.SetString(GAME_STATE_KEY, json);

        return Observable.Return(true);
    }
    public Observable<bool> ResetGameState()
    {
        GameState = CreateGameStateFromSettings();
        SaveGameState();
        return Observable.Return(true);
    }
    private GameStateProxy CreateGameStateFromSettings()
    {
        // —юда можно записать состо€ние по умолчанию
        _gameStateOrigin = new();

        return new GameStateProxy(_gameStateOrigin);
    }

    public Observable<SettingsStateProxy> LoadSettingsState()
    {
        if (!PlayerPrefs.HasKey(SETTINGS_STATE_KEY))
        {
            SettingsState = CreateSettingsStateFromSettings();

            SaveSettingsState();
        }
        else
        {
            var json = PlayerPrefs.GetString(SETTINGS_STATE_KEY);
            _settingsStateOrigin = JsonUtility.FromJson<SettingsState>(json);
            SettingsState = new SettingsStateProxy(_settingsStateOrigin);
        }

        return Observable.Return(SettingsState);
    }
    public Observable<bool> SaveSettingsState()
    {
        var json = JsonUtility.ToJson(_settingsStateOrigin, true);
        PlayerPrefs.SetString(SETTINGS_STATE_KEY, json);

        return Observable.Return(true);
    }
    public Observable<bool> ResetSettingsState()
    {
        SettingsState = CreateSettingsStateFromSettings();
        SaveSettingsState();
        return Observable.Return(true);
    }
    private SettingsStateProxy CreateSettingsStateFromSettings()
    {
        // —юда можно записать состо€ние по умолчанию
        _settingsStateOrigin = new();

        return new SettingsStateProxy(_settingsStateOrigin);
    }
}