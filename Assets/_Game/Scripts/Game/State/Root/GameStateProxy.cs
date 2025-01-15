using R3;
using System.Linq;
using ObservableCollections;

public class GameStateProxy
{
    private readonly GameState _gameState;

    public ObservableList<CreatureEntityProxy> Creatures { get; } = new();

    public GameStateProxy(GameState gameState)
    {
        this._gameState = gameState;
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