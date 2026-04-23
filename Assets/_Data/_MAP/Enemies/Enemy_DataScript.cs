using UnityEngine;

[CreateAssetMenu(fileName = "Enemy_Data", menuName = "Prefabs/Enemies/Enemy")]
public class Enemy_DataScript : ScriptableObject
{
    public int Health;

    public int Speed;
    public int Acceleration;

    public int attackDamage;
    public float attackCooldown;
    public float attackRange;
    public float attackRangeY;
    public float stunTime;

    public GameObject enemiePrefab;
}