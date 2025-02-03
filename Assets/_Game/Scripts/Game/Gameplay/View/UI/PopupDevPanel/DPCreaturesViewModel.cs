
using R3;
using System;
using System.Collections.Generic;
using UnityEngine;

public class DPCreaturesViewModel
{
    private readonly CreaturesSerivce _creaturesSerivce;
    private readonly InputRequests _inputRequests;
    private readonly PopupDevPanelViewModel _parrent;
    public int CurrentCreatureType;
    public List<string> CreatureTypes = new();

    public DPCreaturesViewModel(CreaturesSerivce creaturesSerivce, InputRequests inputRequests, PopupDevPanelViewModel parrent)
    {
        this._creaturesSerivce = creaturesSerivce;
        _inputRequests = inputRequests;
        this._parrent = parrent;
        foreach (var config in creaturesSerivce.CreatureConfigMap)
        {
            CreatureTypes.Add(config.Key);
        }

    }

    public void ToggleCreateCreatureMode()
    {
        if (CurrentCreatureType == 0)
            return;

        _inputRequests.MouseRequest.Subscribe(_ => CreateCreatureAtMousePos(CurrentCreatureType));
        _parrent.RequestClose();
    }

    public void TogglePriveleges()
    {
        _parrent.PrivilegesRequest.OnNext(Unit.Default);
    }

    private void CreateCreatureAtMousePos(int index)
    {
        _creaturesSerivce.CreateCreature(CreatureTypes[index - 1], Camera.main.ScreenToWorldPoint(Input.mousePosition));
    }

}