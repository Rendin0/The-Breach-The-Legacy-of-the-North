using R3;
using ObservableCollections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CreaturesSerivce
{
    private readonly ICommandProcessor _commandProcessor;

    private readonly Dictionary<int, CreatureViewModel> _creaturesMap = new();
    private readonly ObservableList<CreatureViewModel> _creatureViewModels = new();

    private readonly Dictionary<string, CreatureConfig> _creatureConfigMap = new();
    private PlayerViewModel _playerViewModel;

    public IObservableCollection<CreatureViewModel> CreatureViewModels => _creatureViewModels;


    public CreaturesSerivce(IObservableCollection<CreatureEntityProxy> creatures, CreaturesConfig creaturesConfig, ICommandProcessor commandProcessor)
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

        var player = _creatureViewModels.FirstOrDefault(c => c.TypeId == CreaturesTypes.Player);
        if (player == null)
        {
            CreateCreature(CreaturesTypes.Player, Vector3.zero);
        }
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

    public PlayerViewModel GetPlayer()
    {
        return _playerViewModel;
    }

    private void CreateCreatureViewModel(CreatureEntityProxy creatureEntityProxy)
    {

        if (creatureEntityProxy.TypeId == CreaturesTypes.Player)
        {
            var playerViewModel = new PlayerViewModel(creatureEntityProxy);
            _creatureViewModels.Add(playerViewModel);
            _creaturesMap[playerViewModel.CreatureId] = playerViewModel;
            _playerViewModel = playerViewModel;
        }
        else
        {
            var creatureViewModel = new CreatureViewModel(creatureEntityProxy);
            _creatureViewModels.Add(creatureViewModel);
            _creaturesMap[creatureViewModel.CreatureId] = creatureViewModel;
        }
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