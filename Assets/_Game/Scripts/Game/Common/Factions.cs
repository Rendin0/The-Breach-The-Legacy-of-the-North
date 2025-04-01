using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public enum Factions
{
    Player,
    Undeads,
    Beasts,
    People
}

public static class FactionManager
{

    public static LayerMask GetEnemies(Factions faction)
    {
        LayerMask mask = faction switch
        {
            Factions.Player => GetMask(Factions.Undeads, Factions.Beasts),
            Factions.Undeads => GetMask(Factions.Player, Factions.People),
            Factions.Beasts => GetMask(Factions.Player, Factions.People, Factions.Undeads),
            Factions.People => GetMask(Factions.Beasts, Factions.Undeads),
            _ => -1,
        };
        return mask;
    }

    private static int GetMask(params Factions[] factions)
    {
        List<string> names = new();
        foreach (var faction in factions)
        {
            names.Add(faction.ToString());
        }

        return LayerMask.GetMask(names.ToArray());
    }
}