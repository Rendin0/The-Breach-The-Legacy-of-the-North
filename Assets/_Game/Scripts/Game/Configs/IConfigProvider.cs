
using System.Threading.Tasks;

public interface IConfigProvider
{
    // Могут быть разные провайдеры конфига.
    // К примеру из облака либо с локального устройства

    GameConfig GameConfig { get; }
    ApplicationConfig ApplicationConfig { get; }

    Task<GameConfig> LoadGameConfig();
}