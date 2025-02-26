using R3;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEntryPoint
{
    private static GameEntryPoint _instance;
    public static Coroutines Coroutines { get; private set; }
    private UIRootView _uiRoot;

    private readonly DIContainer _rootContainer = new();
    private DIContainer _cachedSceneContainer;


    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void AutoStartGame()
    {
        Application.targetFrameRate = 80;

        _instance = new GameEntryPoint();
        _instance.RunGame();
    }

    private GameEntryPoint()
    {
        Coroutines = new GameObject("[COROUTINES]").AddComponent<Coroutines>();
        Object.DontDestroyOnLoad(Coroutines.gameObject);

        var monoTimer = new GameObject("[MONO TIMER]").AddComponent<MonoTimer>();

        var prefabUIRoot = Resources.Load<UIRootView>("UI/UIRoot");
        _uiRoot = Object.Instantiate(prefabUIRoot);
        Object.DontDestroyOnLoad(_uiRoot.gameObject);

        var configProvider = new LocalConfigProvider();
        _rootContainer.RegisterInstance<IConfigProvider>(configProvider);

        var gameStateProvider = new PlayerPrefsGameStateProvider();
        // Настройки
        gameStateProvider.LoadSettingsState();

        _rootContainer.RegisterInstance<IGameStateProvider>(gameStateProvider);
        _rootContainer.RegisterInstance(_uiRoot);

        var inputRequests = new InputRequests()
        {
            TabRequest = new(),
            EscapeRequest = new(),
            URequest = new(),
            MouseRequest = new(),
            AltRequest = new(),
            MRequest = new(),
        };
        _rootContainer.RegisterInstance(inputRequests);

        var input = new GameInput();
        _rootContainer.RegisterInstance(input);
        _rootContainer.RegisterInstance(new GameplayInputController(input, inputRequests));
        _rootContainer.RegisterInstance(new UIInputController(input, inputRequests.EscapeRequest));

    }

    private async void RunGame()
    {
        await _rootContainer.Resolve<IConfigProvider>().LoadGameConfig();

#if UNITY_EDITOR
        var sceneName = SceneManager.GetActiveScene().name;
        if (sceneName == Scenes.GAMEPLAY)
        {
            // Параметры запуска геймплея сюда
            var gameplayEnterParams = new GameplayEnterParams();

            Coroutines.StartCoroutine(LoadAndStartGameplay(gameplayEnterParams));
            return;
        }

        if (sceneName == Scenes.MAINMENU)
        {
            Coroutines.StartCoroutine(LoadAndStartMainMenu());
            return;
        }

        if (sceneName != Scenes.BOOT)
        {
            return;
        }
#endif
        Coroutines.StartCoroutine(LoadAndStartMainMenu());
    }

    private IEnumerator LoadAndStartGameplay(GameplayEnterParams gameplayEnterParams)
    {
        _uiRoot.ShowLoadingScreen();
        _cachedSceneContainer?.Dispose();

        var input = _rootContainer.Resolve<GameInput>();
        input.UI.Disable();
        input.Player.Enable();

        yield return LoadScene(Scenes.BOOT);
        yield return LoadScene(Scenes.GAMEPLAY);

        // Пропуск кадра, ибо новая сцена может загрузиться до выгрузки старой
        yield return null;

        var isGameStateLoaded = false;
        _rootContainer.Resolve<IGameStateProvider>().LoadGameState().Subscribe(_ => isGameStateLoaded = true);
        yield return new WaitUntil(() => isGameStateLoaded);

        var sceneEntryPoint = Object.FindFirstObjectByType<GameplayEntryPoint>();

        var sceneContainer = _cachedSceneContainer = new DIContainer(_rootContainer);

        sceneEntryPoint.Run(sceneContainer, gameplayEnterParams).Subscribe(gameplayExitParams =>
        {
            Coroutines.StartCoroutine(LoadAndStartMainMenu(gameplayExitParams.ExitParams));
        });

        _uiRoot.HideLoadingScreen();
    }

    private IEnumerator LoadAndStartMainMenu(MainMenuEnterParams enterParams = null)
    {
        _uiRoot.ShowLoadingScreen();
        _cachedSceneContainer?.Dispose();

        var input = _rootContainer.Resolve<GameInput>();
        input.UI.Enable();
        input.Player.Disable();

        yield return LoadScene(Scenes.BOOT);
        yield return LoadScene(Scenes.MAINMENU);

        // Пропуск кадра, ибо новая сцена может загрузиться до выгрузки старой
        yield return null;

        // TODO: DI Container
        var sceneEntryPoint = Object.FindFirstObjectByType<MainMenuEntryPoint>();

        var sceneContainer = _cachedSceneContainer = new DIContainer(_rootContainer);


        sceneEntryPoint.Run(sceneContainer, enterParams).Subscribe(mainMenuExitParams =>
        {
            var sceneName = mainMenuExitParams.ExitParams.SceneName;

            // Тут можно делать переход в разные сцены, в зависимости от имени

            if (sceneName == Scenes.GAMEPLAY)
            {
                Coroutines.StartCoroutine(LoadAndStartGameplay(mainMenuExitParams.ExitParams.As<GameplayEnterParams>()));
            }

        });



        _uiRoot.HideLoadingScreen();
    }

    private IEnumerator LoadScene(string sceneName)
    {
        yield return SceneManager.LoadSceneAsync(sceneName);
    }
}
