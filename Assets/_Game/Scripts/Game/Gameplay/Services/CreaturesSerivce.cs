using R3;
using ObservableCollections;
using System.Collections.Generic;
using UnityEngine;

public class CreaturesSerivce
{
    private readonly ICommandProcessor _commandProcessor;

    private readonly Dictionary<int, CreatureViewModel> _creaturesMap = new();
    private readonly ObservableList<CreatureViewModel> _creatureViewModels = new();

    private readonly Dictionary<string, CreatureConfig> _creatureConfigMap = new();

    public IObservableCollection<CreatureViewModel> CreatureViewModels => _creatureViewModels;


    public CreaturesSerivce(IObservableCollection<CreatureEntityProxy> creatures, CreaturesConfig creaturesConfig,ICommandProcessor commandProcessor)
    {
        _commandProcessor = commandProcessor;

        foreach (var config in creaturesConfig.Creatures)
        {
            _creatureConfigMap[config.TypeId] = config;
        }

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

    public bool CreateCreature(string typeId, Vector3 position)
    {
        var cmd = new CmdCreateCreature(typeId, position);
        var result = _commandProcessor.Process(cmd);

        return result;
    }

    private void CreateCreatureViewModel(CreatureEntityProxy creatureEntityProxy)
    {
        var config = _creatureConfigMap[creatureEntityProxy.TypeId];
        var creatureViewModel = new CreatureViewModel(creatureEntityProxy, config, this);
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