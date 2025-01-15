
using System.Threading.Tasks;
using UnityEngine;

public class LocalConfigProvider : IConfigProvider
{
    private GameConfig _gameConfig;
    public GameConfig GameConfig => _gameConfig;
    public ApplicationConfig ApplicationConfig { get; }

    public LocalConfigProvider()
    {
        ApplicationConfig = Resources.Load<ApplicationConfig>("ApplicationConfig");
    }

    public Task<GameConfig> LoadGameConfig()
    {
        _gameConfig = Resources.Load<GameConfig>("GameConfig");

        return Task.FromResult(_gameConfig);
    }
}