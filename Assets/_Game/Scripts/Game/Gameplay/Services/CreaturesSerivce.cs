using ObservableCollections;
using R3;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// ������ ��� ���������� ���������� � ����.
/// </summary>
public class CreaturesSerivce
{
    private readonly ICommandProcessor _commandProcessor;
    private readonly Dictionary<int, CreatureViewModel> _creaturesMap = new();
    private readonly ObservableList<CreatureViewModel> _creatureViewModels = new();
    private PlayerViewModel _playerViewModel;

    public readonly Dictionary<string, CreatureConfig> CreatureConfigMap = new();

    private readonly AbilitiesConfig _abilitiesConfig;

    /// <summary>
    /// ��������� ������� ������������� �������.
    /// </summary>
    public IObservableCollection<CreatureViewModel> CreatureViewModels => _creatureViewModels;

    /// <summary>
    /// ����������� ������� �������.
    /// </summary>
    /// <param name="creatures">��������� ������ ��������� �������.</param>
    /// <param name="creaturesConfig">������������ �������.</param>
    /// <param name="abilitiesConfig">������������ ������������.</param>
    /// <param name="commandProcessor">��������� ������.</param>
    public CreaturesSerivce(IObservableCollection<CreatureEntityProxy> creatures, CreaturesConfig creaturesConfig, AbilitiesConfig abilitiesConfig, ICommandProcessor commandProcessor)
    {
        _abilitiesConfig = abilitiesConfig;
        _commandProcessor = commandProcessor;

        // ���������� ����� ������������ �������
        foreach (var config in creaturesConfig.Creatures)
        {
            CreatureConfigMap[config.TypeId] = config;
        }

        // �������� ������� ������������� ��� �������
        foreach (var creature in creatures)
        {
            CreateCreatureViewModel(creature);
        }

        // �������� �� ���������� �������
        creatures.ObserveAdd().Subscribe(c =>
        {
            CreateCreatureViewModel(c.Value);
        });

        // �������� �� �������� �������
        creatures.ObserveRemove().Subscribe(c =>
        {
            RemoveCreatureViewModel(c.Value);
        });

        // �������� ������, ���� �� �� ����������
        var player = _creatureViewModels.FirstOrDefault(c => c.TypeId == CreaturesTypes.Player);
        if (player == null)
        {
            CreateCreature(CreaturesTypes.Player, Vector3.zero);
        }
    }

    /// <summary>
    /// ��������� ����� ��������.
    /// </summary>
    /// <param name="creature">������ ������������� ��������.</param>
    /// <param name="damage">������ �� �����.</param>
    /// <returns>��������� ���������� �������.</returns>
    public bool DamageCreature(CreatureViewModel creature, CreatureViewModel damageDealer, DamageData damage)
    {
        var command = new CmdDamageCreature(creature, damageDealer, damage);
        var result = _commandProcessor.Process(command);
        return result;
    }

    public bool HealCreature(CreatureViewModel creature, CreatureViewModel healDealer, float heal)
    {
        var command = new CmdHealCreature(creature, healDealer, heal);
        var result = _commandProcessor.Process(command);

        return result;
    }

    /// <summary>
    /// �������� ��������.
    /// </summary>
    /// <param name="typeId">��� ��������.</param>
    /// <param name="position">������� ��������.</param>
    /// <returns>��������� ���������� �������.</returns>
    public bool CreateCreature(string typeId, Vector3 position)
    {
        var cmd = new CmdCreateCreature(typeId, position);
        var result = _commandProcessor.Process(cmd);

        return result;
    }

    /// <summary>
    /// ��������� ������ ������������� ������.
    /// </summary>
    /// <returns>������ ������������� ������.</returns>
    public PlayerViewModel GetPlayer()
    {
        return _playerViewModel;
    }

    /// <summary>
    /// �������� ��������.
    /// </summary>
    /// <param name="creatureId">ID ��������.</param>
    /// <returns>��������� ���������� �������.</returns>
    private bool KillCreature(int creatureId)
    {
        var cmd = new CmdKillCreature(creatureId);
        var result = _commandProcessor.Process(cmd);
        return result;
    }

    /// <summary>
    /// �������� ��������.
    /// </summary>
    /// <param name="id">ID ��������.</param>
    /// <returns>��������� ���������� �������.</returns>
    private bool DeleteCreature(int id)
    {
        var cmd = new CmdDeleteCreature(id);
        var result = _commandProcessor.Process(cmd);

        return result;
    }

    /// <summary>
    /// �������� ������ ������������� ��������.
    /// </summary>
    /// <param name="creatureEntityProxy">������ �������� ��������.</param>
    private void CreateCreatureViewModel(CreatureEntityProxy creatureEntityProxy)
    {
        if (creatureEntityProxy.TypeId == CreaturesTypes.Player)
        {
            var playerViewModel = new WarriorViewModel(creatureEntityProxy, _abilitiesConfig);
            playerViewModel.CreatureRequests.DeleteRequest.Subscribe(_ =>
            {
                Debug.LogWarning("Trying to delete player??");
            });

            _playerViewModel = playerViewModel;
            _creaturesMap[playerViewModel.CreatureId] = playerViewModel;
            _creatureViewModels.Add(playerViewModel);
        }
        else
        {
            var agentViewModel = new PigAgentViewModel(creatureEntityProxy, _abilitiesConfig);

            agentViewModel.CreatureRequests.DeleteRequest.Subscribe(_ =>
            {
                DeleteCreature(agentViewModel.CreatureId);
            });
            agentViewModel.CreatureRequests.KillRequest.Subscribe(_ =>
            {
                KillCreature(agentViewModel.CreatureId);
            });

            _creaturesMap[agentViewModel.CreatureId] = agentViewModel;
            _creatureViewModels.Add(agentViewModel);
        }
    }

    /// <summary>
    /// �������� ������ ������������� ��������.
    /// </summary>
    /// <param name="creatureEntityProxy">������ �������� ��������.</param>
    private void RemoveCreatureViewModel(CreatureEntityProxy creatureEntityProxy)
    {
        if (_creaturesMap.TryGetValue(creatureEntityProxy.Id, out var creatureViewModel))
        {
            _creaturesMap.Remove(creatureEntityProxy.Id);
            _creatureViewModels.Remove(creatureViewModel);
        }
    }
}
