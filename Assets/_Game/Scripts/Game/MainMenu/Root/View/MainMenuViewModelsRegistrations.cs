using R3;

public static class MainMenuViewModelsRegistrations
{
    public static void Register(DIContainer sceneContainer)
    {
        sceneContainer.RegisterFactory(c => new MainMenuUIManager(sceneContainer)).AsSingle();

        var inputRequests = new InputRequests()
        {
            TabRequest = sceneContainer.Resolve<Subject<Unit>>(AppConstants.TAB_REQUEST_TAG),
            EscapeRequest = sceneContainer.Resolve<Subject<Unit>>(AppConstants.ESCAPE_REQUEST_TAG),
            URequest = sceneContainer.Resolve<Subject<Unit>>(AppConstants.U_REQUEST_TAG),
            MouseRequest = sceneContainer.Resolve<Subject<Unit>>(AppConstants.MOUSE_REQUEST_TAG)
        };
        sceneContainer.RegisterFactory(c => new UIMainMenuRootViewModel(inputRequests)).AsSingle();
    }

}
