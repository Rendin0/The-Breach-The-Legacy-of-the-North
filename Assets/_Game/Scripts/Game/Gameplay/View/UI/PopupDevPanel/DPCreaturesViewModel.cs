
using R3;
using System;
using UnityEngine;

public class DPCreaturesViewModel
{
    private readonly CreaturesSerivce _creaturesSerivce;
    private readonly InputRequests _inputRequests;
    private readonly PopupDevPanelViewModel _parrent;

    public DPCreaturesViewModel(CreaturesSerivce creaturesSerivce, InputRequests inputRequests, PopupDevPanelViewModel parrent)
    {
        this._creaturesSerivce = creaturesSerivce;
        _inputRequests = inputRequests;
        this._parrent = parrent;
    }

    public void ToggleCreateCreatureMode()
    {
        _inputRequests.MouseRequest.Subscribe(_ => CreateCreatureAtMousePos());
        _parrent.RequestClose();
    }

    private void CreateCreatureAtMousePos()
    {
        _creaturesSerivce.CreateCreature("Skeleton", Camera.main.ScreenToWorldPoint(Input.mousePosition));
    }
}