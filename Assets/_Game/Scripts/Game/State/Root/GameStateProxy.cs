using R3;
using System.Linq;
using ObservableCollections;
using System.Collections.Generic;

public class GameStateProxy
{
    private readonly GameState _gameState;

    public ObservableList<CreatureEntityProxy> Creatures { get; } = new();
    public ObservableList<InventoryGrid> Inventories { get; } = new();

    public GameStateProxy(GameState gameState)
    {
        this._gameState = gameState;
        SubscribeCreatures(gameState);
        SubscribeInventories(gameState);
    }

    public void SubscribeInventories(GameState gameState)
    {
        gameState.Inventories.ForEach(c => Inventories.Add(new InventoryGrid(c)));

        Inventories.ObserveAdd().Subscribe(ev =>
        {
            var addedInventoryProxy = ev.Value;

            gameState.Inventories.Add(addedInventoryProxy.Origin);
        });

        Inventories.ObserveRemove().Subscribe(ev =>
        {
            var removedInventoryProxy = ev.Value;
            var removedInventory = gameState.Inventories.FirstOrDefault(c => c.OwnerId == removedInventoryProxy.OwnerId);

            gameState.Inventories.Remove(removedInventory);
        });
    }
    public void SubscribeCreatures(GameState gameState)
    {
        gameState.Creatures.ForEach(c => Creatures.Add(new CreatureEntityProxy(c)));

        Creatures.ObserveAdd().Subscribe(ev =>
        {
            var addedCreatureProxy = ev.Value;

            gameState.Creatures.Add(addedCreatureProxy.Origin);
        });

        Creatures.ObserveRemove().Subscribe(ev =>
        {
            var removedCreatureProxy = ev.Value;
            var removedCreature = gameState.Creatures.FirstOrDefault(c => c.Id == removedCreatureProxy.Id);

            gameState.Creatures.Remove(removedCreature);
        });
    }

    public int GetEntityId()
    {
        return _gameState.GetEntityId();
    }
}