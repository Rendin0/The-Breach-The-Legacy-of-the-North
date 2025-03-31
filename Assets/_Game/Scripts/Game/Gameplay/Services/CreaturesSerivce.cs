using ObservableCollections;
using R3;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Сервис для управления существами в игре.
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
    /// Коллекция моделей представлений существ.
    /// </summary>
    public IObservableCollection<CreatureViewModel> CreatureViewModels => _creatureViewModels;

    /// <summary>
    /// Конструктор сервиса существ.
    /// </summary>
    /// <param name="creatures">Коллекция прокси сущностей существ.</param>
    /// <param name="creaturesConfig">Конфигурация существ.</param>
    /// <param name="abilitiesConfig">Конфигурация способностей.</param>
    /// <param name="commandProcessor">Процессор команд.</param>
    public CreaturesSerivce(IObservableCollection<CreatureEntityProxy> creatures, CreaturesConfig creaturesConfig, AbilitiesConfig abilitiesConfig, ICommandProcessor commandProcessor)
    {
        _abilitiesConfig = abilitiesConfig;
        _commandProcessor = commandProcessor;

        // Заполнение карты конфигураций существ
        foreach (var config in creaturesConfig.Creatures)
        {
            CreatureConfigMap[config.TypeId] = config;
        }

        // Создание моделей представлений для существ
        foreach (var creature in creatures)
        {
            CreateCreatureViewModel(creature);
        }

        // Подписка на добавление существ
        creatures.ObserveAdd().Subscribe(c =>
        {
            CreateCreatureViewModel(c.Value);
        });

        // Подписка на удаление существ
        creatures.ObserveRemove().Subscribe(c =>
        {
            RemoveCreatureViewModel(c.Value);
        });

        // Создание игрока, если он не существует
        var player = _creatureViewModels.FirstOrDefault(c => c.TypeId == CreaturesTypes.Player);
        if (player == null)
        {
            CreateCreature(CreaturesTypes.Player, Vector3.zero);
        }
    }

    /// <summary>
    /// Нанесение урона существу.
    /// </summary>
    /// <param name="creature">Модель представления существа.</param>
    /// <param name="damage">Данные об уроне.</param>
    /// <returns>Результат выполнения команды.</returns>
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
    /// Создание существа.
    /// </summary>
    /// <param name="typeId">Тип существа.</param>
    /// <param name="position">Позиция существа.</param>
    /// <returns>Результат выполнения команды.</returns>
    public bool CreateCreature(string typeId, Vector3 position)
    {
        var cmd = new CmdCreateCreature(typeId, position);
        var result = _commandProcessor.Process(cmd);

        return result;
    }

    /// <summary>
    /// Получение модели представления игрока.
    /// </summary>
    /// <returns>Модель представления игрока.</returns>
    public PlayerViewModel GetPlayer()
    {
        return _playerViewModel;
    }

    /// <summary>
    /// Убийство существа.
    /// </summary>
    /// <param name="creatureId">ID существа.</param>
    /// <returns>Результат выполнения команды.</returns>
    private bool KillCreature(int creatureId)
    {
        var cmd = new CmdKillCreature(creatureId);
        var result = _commandProcessor.Process(cmd);
        return result;
    }

    /// <summary>
    /// Удаление существа.
    /// </summary>
    /// <param name="id">ID существа.</param>
    /// <returns>Результат выполнения команды.</returns>
    private bool DeleteCreature(int id)
    {
        var cmd = new CmdDeleteCreature(id);
        var result = _commandProcessor.Process(cmd);

        return result;
    }

    /// <summary>
    /// Создание модели представления существа.
    /// </summary>
    /// <param name="creatureEntityProxy">Прокси сущности существа.</param>
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
    /// Удаление модели представления существа.
    /// </summary>
    /// <param name="creatureEntityProxy">Прокси сущности существа.</param>
    private void RemoveCreatureViewModel(CreatureEntityProxy creatureEntityProxy)
    {
        if (_creaturesMap.TryGetValue(creatureEntityProxy.Id, out var creatureViewModel))
        {
            _creaturesMap.Remove(creatureEntityProxy.Id);
            _creatureViewModels.Remove(creatureViewModel);
        }
    }
}
