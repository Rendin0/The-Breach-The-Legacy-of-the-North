using R3;
using System.Linq;
using ObservableCollections;

public class GameStateProxy
{
    public ObservableList<CreatureEntityProxy> Creatures { get; } = new();

    public GameStateProxy(GameState creaturesState)
    {
        creaturesState.Creatures.ForEach(c => Creatures.Add(new CreatureEntityProxy(c)));

        Creatures.ObserveAdd().Subscribe(ev =>
        {
            var addedCreatureProxy = ev.Value;

            creaturesState.Creatures.Add(new CreatureEntity
            {
                Id = addedCreatureProxy.Id,
                TypeId = addedCreatureProxy.TypeId,
                MaxHealth = addedCreatureProxy.MaxHealth.Value,
                Health = addedCreatureProxy.Health.Value,
                Position = addedCreatureProxy.Position.Value
            });
        });

        Creatures.ObserveRemove().Subscribe(ev =>
        {
            var removedCreatureProxy = ev.Value;
            var removedCreature = creaturesState.Creatures.FirstOrDefault(c => c.Id == removedCreatureProxy.Id);

            creaturesState.Creatures.Remove(removedCreature);
        });
    }
}