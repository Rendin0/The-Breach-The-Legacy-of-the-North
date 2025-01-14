using R3;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEntryPoint
{
    private static GameEntryPoint _instance;
    private Coroutines _coroutines;
    private UIRootView _uiRoot;

    private readonly DIContainer _rootContainer = new();
    private DIContainer _cachedSceneContainer;


    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void AutoStartGame()
    {
        Application.targetFrameRate = 60;

        _instance = new GameEntryPoint();
        _instance.RunGame();
    }

    private GameEntryPoint()
    {
        _coroutines = new GameObject("[COROUTINES]").AddComponent<Coroutines>();
        Object.DontDestroyOnLoad(_coroutines.gameObject);

        var prefabUIRoot = Resources.Load<UIRootView>("UI/UIRoot");
        _uiRoot = Object.Instantiate(prefabUIRoot);
        Object.DontDestroyOnLoad(_uiRoot.gameObject);

        var gameStateProvider = new PlayerPrefsGameStateProvider();

        // ���������
        gameStateProvider.LoadSettingsState();
        
        _rootContainer.RegisterInstance<IGameStateProvider>(gameStateProvider);
        _rootContainer.RegisterInstance(_uiRoot);
    }

    private void RunGame()
    {
#if UNITY_EDITOR
        var sceneName = SceneManager.GetActiveScene().name;
        if (sceneName == Scenes.GAMEPLAY)
        {
            // ��������� ������� �������� ����
            var gameplayEnterParams = new GameplayEnterParams();

            _coroutines.StartCoroutine(LoadAndStartGameplay(gameplayEnterParams));
            return;
        }

        if (sceneName == Scenes.MAINMENU)
        {
            _coroutines.StartCoroutine(LoadAndStartMainMenu());
            return;
        }

        if (sceneName != Scenes.BOOT)
        {
            return;
        }
#endif
        _coroutines.StartCoroutine(LoadAndStartMainMenu());
    }

    private IEnumerator LoadAndStartGameplay(GameplayEnterParams gameplayEnterParams)
    {
        _uiRoot.ShowLoadingScreen();
        _cachedSceneContainer?.Dispose();


        yield return LoadScene(Scenes.BOOT);
        yield return LoadScene(Scenes.GAMEPLAY);

        // ������� �����, ��� ����� ����� ����� ����������� �� �������� ������
        yield return null;

        var isGameStateLoaded = false;
        _rootContainer.Resolve<IGameStateProvider>().LoadGameState().Subscribe(_ => isGameStateLoaded = true);
        yield return new WaitUntil(() =>  isGameStateLoaded);

        var sceneEntryPoint = Object.FindFirstObjectByType<GameplayEntryPoint>();

        var sceneContainer = _cachedSceneContainer = new DIContainer(_rootContainer);

        sceneEntryPoint.Run(sceneContainer, gameplayEnterParams).Subscribe(gameplayExitParams =>
        {
            _coroutines.StartCoroutine(LoadAndStartMainMenu(gameplayExitParams.ExitParams));
        });

        _uiRoot.HideLoadingScreen();
    }

    private IEnumerator LoadAndStartMainMenu(MainMenuEnterParams enterParams = null)
    {
        _uiRoot.ShowLoadingScreen();
        _cachedSceneContainer?.Dispose();

        yield return LoadScene(Scenes.BOOT);
        yield return LoadScene(Scenes.MAINMENU);

        // ������� �����, ��� ����� ����� ����� ����������� �� �������� ������
        yield return null;

        // TODO: DI Container
        var sceneEntryPoint = Object.FindFirstObjectByType<MainMenuEntryPoint>();

        var sceneContainer = _cachedSceneContainer = new DIContainer(_rootContainer);


        sceneEntryPoint.Run(sceneContainer, enterParams).Subscribe(mainMenuExitParams =>
        {
            var sceneName = mainMenuExitParams.ExitParams.SceneName;

            // ��� ����� ������ ������� � ������ �����, � ����������� �� �����

            if (sceneName == Scenes.GAMEPLAY)
            {
                _coroutines.StartCoroutine(LoadAndStartGameplay(mainMenuExitParams.ExitParams.As<GameplayEnterParams>()));
            }

        });



        _uiRoot.HideLoadingScreen();
    }

    private IEnumerator LoadScene(string sceneName)
    {
        yield return SceneManager.LoadSceneAsync(sceneName);
    }
}
