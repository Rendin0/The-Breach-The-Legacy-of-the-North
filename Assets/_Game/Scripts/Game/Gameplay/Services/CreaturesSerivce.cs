using R3;
using ObservableCollections;
using System.Collections.Generic;
using UnityEngine;

public class CreaturesSerivce
{
    private readonly ICommandProcessor _commandProcessor;
    private readonly Dictionary<int, CreatureViewModel> _creaturesMap = new();

    private readonly ObservableList<CreatureViewModel> _creatureViewModels = new();

    public IObservableCollection<CreatureViewModel> CreatureViewModels => _creatureViewModels;


    public CreaturesSerivce(IObservableCollection<CreatureEntityProxy> creatures, ICommandProcessor commandProcessor)
    {
        _commandProcessor = commandProcessor;

        foreach (var creature in creatures)
        {
            CreateCreatureViewModel(creature);
        }

        creatures.ObserveAdd().Subscribe(c =>
        {
            CreateCreatureViewModel(c.Value);
        });

        creatures.ObserveRemove().Subscribe(c =>
        {
            RemoveCreatureViewModel(c.Value);
        });
    }

    public bool DamageCreature(int creatureId, float damage)
    {
        var command = new CmdDamageCreature(creatureId, damage);
        var result = _commandProcessor.Process(command);
        return result;
    }

    public bool CreateCreature(string typeId, Vector3 position, float healt, float maxHealth)
    {
        var cmd = new CmdCreateCreature(typeId, position, healt, maxHealth);
        var result = _commandProcessor.Process(cmd);

        return result;
    }

    private void CreateCreatureViewModel(CreatureEntityProxy creatureEntityProxy)
    {
        var creatureViewModel = new CreatureViewModel(creatureEntityProxy, this);
        _creatureViewModels.Add(creatureViewModel);
        _creaturesMap[creatureViewModel.CreatureId] = creatureViewModel;
    }

    private void RemoveCreatureViewModel(CreatureEntityProxy creatureEntityProxy)
    {
        if (_creaturesMap.TryGetValue(creatureEntityProxy.Id, out var creatureViewModel))
        {
            _creaturesMap.Remove(creatureEntityProxy.Id);
            _creatureViewModels.Remove(creatureViewModel);
        }
    }

}