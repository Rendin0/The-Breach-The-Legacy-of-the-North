
using CrashKonijn.Goap.Runtime;
using ObservableCollections;
using R3;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GoapBehaviour), typeof(AgentTypeFactoryBase), typeof(ProactiveControllerBehaviour))]
public class WorldGameplayRootBinder : MonoBehaviour
{
    private readonly Dictionary<int, CreatureBinder> _creatures = new();
    private readonly CompositeDisposable _disposables = new();
    private GameplayUIManager _uiManager;

    private GoapBehaviour _goapBehaviour;
    private AgentTypeFactoryBase _agentTypeFactory;

    private void Awake()
    {
        _goapBehaviour = GetComponent<GoapBehaviour>();
        _agentTypeFactory = GetComponent<AgentTypeFactoryBase>();
    }

    public void Bind(WorldGameplayRootViewModel viewModel, GameplayUIManager uiManager)
    {
        _uiManager = uiManager;

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

    private void OnDestroy()
    {
        _disposables.Dispose();
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
        if (created.TryGetComponent<AgentTypeBinder>(out var setBinder))
        {
            setBinder.GoapBehaviour = _goapBehaviour;
        }

        created.Bind(viewModel);
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

}