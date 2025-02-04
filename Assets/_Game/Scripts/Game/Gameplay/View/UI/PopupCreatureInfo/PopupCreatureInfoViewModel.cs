
using System.Collections.Generic;
using static UnityEngine.Rendering.STP;

public class PopupCreatureInfoViewModel : WindowViewModel
{
    private readonly InventoriesService _inventoriesService;

    public readonly CreatureViewModel CreatureViewModel;

    public List<string> ItemTypes;

    public override string Id => "PopupCreatureInfo";

    public PopupCreatureInfoViewModel(CreatureViewModel creatureViewModel, InventoriesService inventoriesService)
    {
        CreatureViewModel = creatureViewModel;
        _inventoriesService = inventoriesService;
        ItemTypes = GetItemTypes();
    }

    public bool AddInventory(int slotsAmount)
    {
        return _inventoriesService.CreateInventory(CreatureViewModel.CreatureId, slotsAmount);
    }

    public PopupInventoryViewModel GetInventory()
    {
        return _inventoriesService.GetInventory(CreatureViewModel.CreatureId);
    }

    public void AddItem(int itemIndex, int amount)
    {
        if (itemIndex > 0)
            _inventoriesService.AddItemInInventory(CreatureViewModel.CreatureId, ItemTypes[itemIndex - 1], amount);
    }

    public void AddSlots(int slotsAmount)
    {
        _inventoriesService.AddSlotsToInventory(CreatureViewModel.CreatureId, slotsAmount);
    }

    private List<string> GetItemTypes()
    {
        List<string> types = new();

        foreach (var config in _inventoriesService.ItemsConfig)
        {
            types.Add(config.Key);
        }
        types.Remove(ItemsIDs.Nothing);

        return types;
    }
}