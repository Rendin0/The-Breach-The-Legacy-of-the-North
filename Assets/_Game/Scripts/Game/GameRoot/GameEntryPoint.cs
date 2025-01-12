using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using R3;

public class GameEntryPoint
{
    private static GameEntryPoint _instance;
    private Coroutines _coroutines;
    private UIRootView _uiRoot;


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

        var prefabUIRoot = Resources.Load<UIRootView>("UIRoot");
        _uiRoot = Object.Instantiate(prefabUIRoot);
        Object.DontDestroyOnLoad(_uiRoot.gameObject);
    }

    private void RunGame()
    {
#if UNITY_EDITOR
        var sceneName = SceneManager.GetActiveScene().name;
        if (sceneName == Scenes.GAMEPLAY)
        {
            // Параметры запуска геймплея сюда
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

        yield return LoadScene(Scenes.BOOT);
        yield return LoadScene(Scenes.GAMEPLAY);

        // Пропуск кадра, ибо новая сцена может загрузиться до выгрузки старой
        yield return null;

        // TODO: DI Container
        var sceneEntryPoint = Object.FindFirstObjectByType<GameplayEntryPoint>();
        sceneEntryPoint.Run(_uiRoot, gameplayEnterParams).Subscribe(gameplayExitParams =>
        {
            _coroutines.StartCoroutine(LoadAndStartMainMenu(gameplayExitParams.ExitParams));
        });

        _uiRoot.HideLoadingScreen();
    }

    private IEnumerator LoadAndStartMainMenu(MainMenuEnterParams enterParams = null)
    {
        _uiRoot.ShowLoadingScreen();

        yield return LoadScene(Scenes.BOOT);
        yield return LoadScene(Scenes.MAINMENU);

        // Пропуск кадра, ибо новая сцена может загрузиться до выгрузки старой
        yield return null;

        // TODO: DI Container
        var sceneEntryPoint = Object.FindFirstObjectByType<MainMenuEntryPoint>();
        sceneEntryPoint.Run(_uiRoot, enterParams).Subscribe(mainMenuExitParams =>
        {
            var sceneName = mainMenuExitParams.ExitParams.SceneName;

            // Тут можно делать переход в разные сцены, в зависимости от имени

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
