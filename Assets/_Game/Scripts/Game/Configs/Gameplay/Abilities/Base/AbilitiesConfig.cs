using UnityEngine;

[CreateAssetMenu(fileName = "AbilitiesConfig", menuName = "Game Config/Abilities/Abilities Config")]
public class AbilitiesConfig : ScriptableObject
{
    public WarriorAbilitesConfig WarriorAbilitesConfig;
    public PigAbilitiesConfig PigAbilitiesConfig;
    public HunterAbilitiesConfig HunterAbilitiesConfig;
}
