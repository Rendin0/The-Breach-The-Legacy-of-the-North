
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CreaturesConfig", menuName = "Game Config/New Creatures Config")]
public class CreaturesConfig : ScriptableObject
{
    public List<CreatureConfig> Creatures;
}