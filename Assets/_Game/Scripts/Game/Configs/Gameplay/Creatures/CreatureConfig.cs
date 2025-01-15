
using UnityEngine;

public enum Fractions
{
    Player,
    Undead
}


[CreateAssetMenu(fileName = "CreatureConfig", menuName = "Game Config/Creatures/New Creature Config")]
public class CreatureConfig : ScriptableObject
{
    public string TypeId;
    [Min(0f)] public float MaxHealth;
    [Min(0f)] public float Health;
    public Fractions Fraction;
}