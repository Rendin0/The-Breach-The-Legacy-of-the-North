using System;
using UnityEngine;

public class UIMainMenuRootBinder : MonoBehaviour
{
    public event Action GoToGameplayButtonClicked;

    public void HandleGoToGameplayButtonClick()
    {
        GoToGameplayButtonClicked?.Invoke();
    }
}
