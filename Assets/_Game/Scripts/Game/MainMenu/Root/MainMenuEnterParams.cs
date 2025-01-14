using UnityEngine;

public class MainMenuEnterParams : SceneEnterParams
{
    public string EnterParams { get; }

    public MainMenuEnterParams(string enterParams) : base(Scenes.MAINMENU)
    {
        this.EnterParams = enterParams;
    }


}
