

using R3;
using UnityEngine;

public class PopupCreatureMenuViewModel : WindowViewModel
{
    public override string Id => "PopupCreatureMenu";

    public readonly string CreatureName;
    public readonly int CreatureId;
    public readonly Vector2 Position;
    private readonly CreatureViewModel _creatureViewModel;
    private readonly GameplayUIManager _uiManager;

    public PopupCreatureMenuViewModel(CreatureViewModel creatureViewModel, Vector2 position, GameplayUIManager uiManager)
    {
        _creatureViewModel = creatureViewModel;
        CreatureId = creatureViewModel.CreatureId;
        CreatureName = creatureViewModel.TypeId;
        Position = position;
        _uiManager = uiManager;
    }

    public void OpenCreatureInfo()
    {
        _uiManager.OpenPopupCreatureInfo(_creatureViewModel);
        RequestClose();
    }

    public void DeleteCreature()
    {
        _creatureViewModel.DeleteRequest.OnNext(_creatureViewModel);
        RequestClose();
    }
}