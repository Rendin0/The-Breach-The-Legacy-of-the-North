using UnityEngine;

public enum Factions
{
    Player,
    Undead
}

public static class FactionManager
{

    public static LayerMask GetEnemies(Factions faction)
    {
        LayerMask mask = faction switch
        {
            Factions.Player => LayerMask.GetMask(Factions.Undead.ToString()),
            Factions.Undead => LayerMask.GetMask(Factions.Player.ToString()),
            _ => -1,
        };
        return mask;
    }
}