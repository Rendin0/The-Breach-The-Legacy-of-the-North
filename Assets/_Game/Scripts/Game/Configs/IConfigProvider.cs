
using System.Threading.Tasks;

public interface IConfigProvider
{
    // ����� ���� ������ ���������� �������.
    // � ������� �� ������ ���� � ���������� ����������

    GameConfig GameConfig { get; }
    ApplicationConfig ApplicationConfig { get; }

    Task<GameConfig> LoadGameConfig();
}