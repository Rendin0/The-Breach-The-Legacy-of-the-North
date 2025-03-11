using ObservableCollections;
using R3;
using System.Collections.Generic;
using UnityEngine;

public class WorldGameplayRootBinder : MonoBehaviour
{
    private readonly Dictionary<int, CreatureBinder> _creatures = new();
    private readonly CompositeDisposable _disposables = new();
    private GameplayUIManager _uiManager;


    private WorldGameplayRootViewModel _viewModel;

    private void OnDestroy()
    {
        _disposables.Dispose();
    }

    public void Bind(WorldGameplayRootViewModel viewModel, GameplayUIManager uiManager)
    {
        _viewModel = viewModel;
        _uiManager = uiManager;
        InitCreatures(viewModel);
    }

    #region Creatures
    private void InitCreatures(WorldGameplayRootViewModel viewModel)
    {
        foreach (var creature in viewModel.CreatureViewModels)
        {
            CreateCreature(creature);
        }

        _disposables.Add(viewModel.CreatureViewModels.ObserveAdd().Subscribe(ev =>
        {
            CreateCreature(ev.Value);
        }));

        _disposables.Add(viewModel.CreatureViewModels.ObserveRemove().Subscribe(ev =>
        {
            DestroyCreature(ev.Value);
        }));
    }
    private void DamageCreature(CreatureViewModel viewModel)
    {

    }
    private void CreateCreature(CreatureViewModel viewModel)
    {
        viewModel.OnCreatureClick.Subscribe(c =>
        {
            _uiManager.OpenPopupCreatureMenu(c);
        });

        var creatureType = viewModel.TypeId;
        var creaturePrefabPath = $"Gameplay/Creatures/Creature_{creatureType}";
        var prefab = Resources.Load<CreatureBinder>(creaturePrefabPath);

        var created = Instantiate(prefab);
        created.Bind(viewModel);

        if (viewModel.TypeId == CreaturesTypes.Player)
            created.gameObject.layer = LayerMask.NameToLayer(LayerNames.Player);
        else
            created.gameObject.layer = LayerMask.NameToLayer(LayerNames.Creatures);

        _creatures[viewModel.CreatureId] = created;
    }
    private void DestroyCreature(CreatureViewModel viewModel)
    {
        if (_creatures.TryGetValue(viewModel.CreatureId, out var creature))
        {
            Destroy(creature.gameObject);
            _creatures.Remove(viewModel.CreatureId);

        }
    }
    #endregion
}