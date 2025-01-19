
using R3;

public class MainMenuUIManager : UIManager
{
    private readonly Subject<Unit> _exitSceneRequest;

    public MainMenuUIManager(DIContainer sceneContainer) : base(sceneContainer)
    {
        _exitSceneRequest = sceneContainer.Resolve<Subject<Unit>>(AppConstants.EXIT_SCENE_REQUEST_TAG);
    }

    public PopupSettingsViewModel OpenPopupSettings()
    {
        var viewModel = new PopupSettingsViewModel();
        var rootUI = Container.Resolve<UIMainMenuRootViewModel>();

        rootUI.OpenPopup(viewModel, null);
        return viewModel;
    }

    public ScreenMainMenuViewModel OpenMainMenu()
    {
        var viewModel = new ScreenMainMenuViewModel(this, _exitSceneRequest);
        var rootUI = Container.Resolve<UIMainMenuRootViewModel>();

        rootUI.OpenScreen(viewModel);
        return viewModel;
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}

