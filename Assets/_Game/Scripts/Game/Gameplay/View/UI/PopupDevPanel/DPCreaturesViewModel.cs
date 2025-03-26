
using R3;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DPCreaturesViewModel
{
    private readonly CreaturesSerivce _creaturesSerivce;
    private readonly InputRequests _inputRequests;
    private readonly PopupDevPanelViewModel _parrent;
    public int CurrentCreatureType;
    public List<string> CreatureTypesList = new();

    public DPCreaturesViewModel(CreaturesSerivce creaturesSerivce, InputRequests inputRequests, PopupDevPanelViewModel parrent)
    {
        this._creaturesSerivce = creaturesSerivce;
        _inputRequests = inputRequests;
        this._parrent = parrent;
        foreach (var config in creaturesSerivce.CreatureConfigMap)
        {
            CreatureTypesList.Add(config.Key);
        }
        CreatureTypesList.Remove(CreaturesTypes.Player);
    }

    public void ToggleCreateCreatureMode()
    {
        if (CurrentCreatureType == 0)
            return;

        _inputRequests.MouseRequest.Subscribe(_ => CreateCreatureAtMousePos(_, CurrentCreatureType));
        _parrent.RequestClose();
    }

    public void TogglePriveleges()
    {
        _parrent.PrivilegesRequest.OnNext(Unit.Default);
    }

    private void CreateCreatureAtMousePos(InputAction.CallbackContext context, int index)
    {
        if (context.performed)
            _creaturesSerivce.CreateCreature(CreatureTypesList[index - 1], Camera.main.ScreenToWorldPoint(Input.mousePosition));
    }

}