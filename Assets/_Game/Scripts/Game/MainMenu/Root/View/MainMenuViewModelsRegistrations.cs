using R3;

public static class MainMenuViewModelsRegistrations
{
    public static void Register(DIContainer sceneContainer)
    {
        sceneContainer.RegisterFactory(c => new MainMenuUIManager(sceneContainer)).AsSingle();

        var escapeSignal = sceneContainer.Resolve<Subject<Unit>>(AppConstants.ESCAPE_REQUEST_TAG);
        var tabSignal = sceneContainer.Resolve<Subject<Unit>>(AppConstants.TAB_REQUEST_TAG);
        sceneContainer.RegisterFactory(c => new UIMainMenuRootViewModel(escapeSignal, tabSignal)).AsSingle();
    }

}
