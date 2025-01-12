using R3;
using System;
using UnityEngine;

public class UIGameplayRootBinder : MonoBehaviour 
{
    private Subject<Unit> _exitSceneSignalSubject;

    public void HandleGoToMainMenuButtonClick()
    {
        _exitSceneSignalSubject?.OnNext(Unit.Default);
    }

    public void Bind(Subject<Unit> exitSceneSignalSubject)
    {
        _exitSceneSignalSubject = exitSceneSignalSubject;
    }
}
