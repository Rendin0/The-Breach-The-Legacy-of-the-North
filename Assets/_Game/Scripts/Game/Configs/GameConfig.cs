
using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "Game Config/New Game Config")]
public class GameConfig : ScriptableObject
{
    // �� ������� �������!
    public CreaturesConfig CreaturesConfig;
    public ItemsConfig ItemsConfig;
    public AbilitiesConfig AbilitiesConfig;

}