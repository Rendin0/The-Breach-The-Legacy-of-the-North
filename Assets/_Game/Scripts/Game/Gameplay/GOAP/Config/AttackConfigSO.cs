using UnityEngine;

[CreateAssetMenu(fileName = "Attack Config", menuName = "AI/Attack Config")]
public class AttackConfigSO : ScriptableObject
{
    public CreatureViewModel Attacker;
    public float SensorRadius = 10f;
    public int MeleeAttackCost = 1;
    public LayerMask AttackLayer;
}