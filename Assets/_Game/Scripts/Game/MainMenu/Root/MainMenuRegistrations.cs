using UnityEngine;
using R3;

public static class MainMenuRegistrations
{
    public static void Register(DIContainer sceneContainer, MainMenuEnterParams enterParams) 
    {
        sceneContainer.RegisterInstance(AppConstants.EXIT_SCENE_REQUEST_TAG, new Subject<Unit>());
    }

}
