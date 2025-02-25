
using System.Collections.Generic;
using UnityEngine;

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
    public void Bind(PopupWorldMapBinder parrent)
    {
        foreach (var button in _regionButtons)
        {
            button.onClick.AddListener(() => ChangeImage(parrent, button));
        }
    }

    private void ChangeImage(PopupWorldMapBinder parrent, NonRectButton button)
    {
        parrent.LoadImage(button);
    }
}