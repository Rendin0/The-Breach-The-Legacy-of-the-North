using ObservableCollections;
using R3;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CreaturesSerivce
{
    private readonly ICommandProcessor _commandProcessor;
    private readonly Dictionary<int, CreatureViewModel> _creaturesMap = new();
    private readonly ObservableList<CreatureViewModel> _creatureViewModels = new();

    public readonly Dictionary<string, CreatureConfig> CreatureConfigMap = new();
    private PlayerViewModel _playerViewModel;

    private readonly AbilitiesConfig _abilitiesConfig;

    public IObservableCollection<CreatureViewModel> CreatureViewModels => _creatureViewModels;


    public CreaturesSerivce(IObservableCollection<CreatureEntityProxy> creatures, CreaturesConfig creaturesConfig, AbilitiesConfig abilitiesConfig, ICommandProcessor commandProcessor)
    {
        _abilitiesConfig = abilitiesConfig;
        _commandProcessor = commandProcessor;
        foreach (var config in creaturesConfig.Creatures)
        {
            CreatureConfigMap[config.TypeId] = config;
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

    private bool DamageCreature(CreatureViewModel creature, DamageData damage)
    {
        var command = new CmdDamageCreature(creature, damage);
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

    private bool KillCreature(int creatureId)
    {
        var cmd = new CmdKillCreature(creatureId);
        var result = _commandProcessor.Process(cmd);
        return result;
    }

    private bool DeleteCreature(int id)
    {
        var cmd = new CmdDeleteCreature(id);
        var result = _commandProcessor.Process(cmd);

        return result;
    }

    private void CreateCreatureViewModel(CreatureEntityProxy creatureEntityProxy)
    {

        if (creatureEntityProxy.TypeId == CreaturesTypes.Player)
        {
            var playerViewModel = new PlayerViewModel(creatureEntityProxy, _abilitiesConfig);
            _creatureViewModels.Add(playerViewModel);
            _creaturesMap[playerViewModel.CreatureId] = playerViewModel;
            playerViewModel.DeleteRequest.Subscribe(_ =>
            {
                Debug.LogWarning("Trying to delete player??");
            });

            _playerViewModel = playerViewModel;
        }
        else
        {
            var creatureViewModel = new CreatureViewModel(creatureEntityProxy);
            _creatureViewModels.Add(creatureViewModel);
            _creaturesMap[creatureViewModel.CreatureId] = creatureViewModel;
            creatureViewModel.AgentType = CreatureConfigMap[creatureViewModel.TypeId].AgentType;

            creatureViewModel.DeleteRequest.Subscribe(_ =>
            {
                DeleteCreature(creatureViewModel.CreatureId);
            });
            creatureViewModel.KillRequest.Subscribe(_ =>
            {
                KillCreature(creatureViewModel.CreatureId);
            });
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