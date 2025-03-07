
using CrashKonijn.Goap.Runtime;
using ObservableCollections;
using R3;
using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GoapBehaviour), typeof(ProactiveControllerBehaviour))]
public class WorldGameplayRootBinder : MonoBehaviour
{
    private readonly Dictionary<int, CreatureBinder> _creatures = new();
    private readonly CompositeDisposable _disposables = new();
    private GameplayUIManager _uiManager;

    private GoapBehaviour _goapBehaviour;

    private WorldGameplayRootViewModel _viewModel;

    private readonly Dictionary<AgentTypes, ATCF> _atcfsMap = new();

    private void Awake()
    {
        _goapBehaviour = GetComponent<GoapBehaviour>();
        InitAgents();
    }
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

    #region GOAP
    private void InitAgents()
    {
        AddATCF<ATCFEnemy>();
    }
    private void AddATCF<T>() where T : ATCF
    {
        var atcf = gameObject.AddComponent<T>();
        _atcfsMap.Add(atcf.AgentType, atcf);
        _goapBehaviour.agentTypeConfigFactories.Add(atcf);
    }
    #endregion

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
        if (created.TryGetComponent<AgentTypeBinder>(out var agentTypeBinder))
        {
            agentTypeBinder.Init(_goapBehaviour, _atcfsMap[viewModel.AgentType].GetBrain());
        }
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