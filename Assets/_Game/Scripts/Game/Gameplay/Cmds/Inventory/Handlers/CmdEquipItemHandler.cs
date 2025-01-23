
using System.Collections.Generic;

public class CmdEquipItemHandler : ICommandHandler<CmdEquipItem>
{
    private readonly ICommandProcessor _commandProcessor;
    private readonly GameStateProxy _gameStateProxy;
    private readonly Dictionary<string, ItemConfig> _itemsConfig = new();

    public CmdEquipItemHandler(ICommandProcessor commandProcessor, GameStateProxy gameStateProxy, ItemsConfig itemsConfig)
    {
        _commandProcessor = commandProcessor;
        _gameStateProxy = gameStateProxy;

        foreach (var itemConfig in itemsConfig.Items)
            _itemsConfig[itemConfig.ItemId] = itemConfig;
    }

    public bool Handle(CmdEquipItem command)
    {
        var equip = command.Inventory.Equipment[command.Equipment];
        var tmpEquipType = equip.ItemId.Value;
        var tmpEquipAmount = equip.Amount.Value;

        var item = command.Item;

        // Тип предмета соответствует слоту
        // Либо пустой слот
        if ((_itemsConfig[item.ItemId.Value] is ItemEquipmentConfig e
            && e.EquipmentType == command.Equipment)
            || item.ItemId.Value == ItemsIDs.Nothing)
        {

            equip.Amount.OnNext(item.Amount.Value);
            equip.ItemId.OnNext(item.ItemId.Value);

            item.Amount.OnNext(tmpEquipAmount);
            item.ItemId.OnNext(tmpEquipType);

            return true;
        }
        return false;
    }
}
