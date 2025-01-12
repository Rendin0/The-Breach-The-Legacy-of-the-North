using UnityEngine;

public class GameplayEnterParams : SceneEnterParams
{
    // Тут можно хранить параметры запуска геймплея
    // Сейв файл, класс, карту и т.п.

    public GameplayEnterParams() : base(Scenes.GAMEPLAY)
    {

    }
}
