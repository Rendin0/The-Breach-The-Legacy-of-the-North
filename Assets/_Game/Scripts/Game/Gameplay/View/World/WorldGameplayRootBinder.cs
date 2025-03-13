using CrashKonijn.Goap.Core;
using CrashKonijn.Goap.Runtime;
using ObservableCollections;
using R3;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Profiling.HierarchyFrameDataView;

public class WorldGameplayRootBinder : MonoBehaviour
{
    private readonly Dictionary<int, CreatureBinder> _creatures = new();
    private readonly CompositeDisposable _disposables = new();
    private GameplayUIManager _uiManager;

    private GoapBehaviour _goap;

    private WorldGameplayRootViewModel _viewModel;

    private readonly Dictionary<string, ATCF> _atcfsMap = new();

    private void OnDestroy()    
    {
        _disposables.Dispose();
    }

    public void Bind(WorldGameplayRootViewModel viewModel, GameplayUIManager uiManager)
    {
        _viewModel = viewModel;
        _uiManager = uiManager;
        InitGoap();
        InitCreatures(viewModel);
    }

    #region GOAP
    private void InitGoap()
    {
        var goap = Resources.Load<GoapBehaviour>("Gameplay/GOAP");
        _goap = Instantiate(goap);

        InitATCFsMap(_goap);
    }

    private void InitATCFsMap(GoapBehaviour goap)
    {
        var atcfs = goap.GetComponentsInChildren<ATCF>();

        foreach (var atcf in atcfs)
        {
            _atcfsMap.Add(atcf.AgentType, atcf);
        }
    }

    private AgentBrain GetBrain(AgentTypes agentType, GameObject go)
    {
        if (_atcfsMap.TryGetValue(agentType.ToSafeString(), out var atcf))
            return atcf.GetBrain(go);

        return null;
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
        created.Bind(viewModel, _goap, GetBrain(viewModel.AgentType, created.gameObject));

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