using UnityEngine;

public class UIRootView : MonoBehaviour 
{
    [SerializeField] private GameObject loadingScreen;

    private void Awake()
    {
        HideLoadingScreen();
    }

    public void ShowLoadingScreen()
    {
        loadingScreen.SetActive(true);
    }
    public void HideLoadingScreen()
    {
        loadingScreen.SetActive(false);
    }
}
