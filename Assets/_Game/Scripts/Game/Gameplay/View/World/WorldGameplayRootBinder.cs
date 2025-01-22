
using ObservableCollections;
using R3;
using System.Collections.Generic;
using UnityEngine;

public class WorldGameplayRootBinder : MonoBehaviour
{
    private readonly Dictionary<int, CreatureBinder> _creatures = new();
    private readonly CompositeDisposable _disposables = new();

    public void Bind(WorldGameplayRootViewModel viewModel)
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

    private void OnDestroy()
    {
        _disposables.Dispose();
    }

    private void DamageCreature(CreatureViewModel viewwModel)
    {

    }

    private void CreateCreature(CreatureViewModel viewModel)
    {
        var creatureType = viewModel.TypeId;
        var creaturePrefabPath = $"Gameplay/Creatures/Creature_{creatureType}";
        var prefab = Resources.Load<CreatureBinder>(creaturePrefabPath);

        var created = Instantiate(prefab);
        created.Bind(viewModel);

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