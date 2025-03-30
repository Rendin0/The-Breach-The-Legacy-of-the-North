using System;
using System.Collections.Generic;
using UnityEngine;

[Obsolete("Old map script")]
public class MapImageBinder : MonoBehaviour
{
    public List<NonRectButton> _regionButtons;

    private void OnDestroy()
    {
        foreach (var button in _regionButtons)
        {
            button.onClick.RemoveAllListeners();
        }
    }
    public void Bind(PopupWorldMapBinderOld parrent)
    {
        foreach (var button in _regionButtons)
        {
            button.onClick.AddListener(() => ChangeImage(parrent, button));
        }
    }

    private void ChangeImage(PopupWorldMapBinderOld parrent, NonRectButton button)
    {
        parrent.LoadImage(button);
    }
}