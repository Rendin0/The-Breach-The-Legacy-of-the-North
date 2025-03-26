
using System;
using UnityEngine;

[Serializable]
public class DamageData
{
    [Min(0f)] public float MagicalData = 0f;
    [Min(0f)] public float PhysicalData = 0f;
}