
using UnityEngine;

public class CmdCreateCreature : ICommand
{
    public readonly string TypeId;
    public readonly Vector3 Position;
    public readonly float Health;
    public readonly float MaxHealth;

    public CmdCreateCreature(string typeId, Vector3 position, float health, float maxHealth)
    {
        TypeId = typeId;
        Position = position;
        Health = health;
        MaxHealth = maxHealth;
    }
}