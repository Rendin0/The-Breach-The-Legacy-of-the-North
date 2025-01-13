using R3;

public static class GameplayRegistrations
{

    public static void Register(DIContainer sceneContainer, GameplayEnterParams enterParams) 
    {
        sceneContainer.RegisterInstance(AppConstants.EXIT_SCENE_REQUEST_TAG, new Subject<Unit>());
    }
}
