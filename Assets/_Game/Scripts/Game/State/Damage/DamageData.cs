
using System;
using UnityEngine;

[Serializable]
public class DamageData
{
    [Min(0f)] public float MagicalData;
    [Min(0f)] public float PhysicalData;
}