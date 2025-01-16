
using UnityEngine;

public class CmdCreateCreature : ICommand
{
    public readonly string TypeId;
    public readonly Vector3 Position;

    public CmdCreateCreature(string typeId, Vector3 position)
    {
        TypeId = typeId;
        Position = position;
    }
}