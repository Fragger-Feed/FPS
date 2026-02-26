using UnityEngine;

[CreateAssetMenu(fileName = "Enemy_Data", menuName = "Prefabs/Enemies/Enemy")]
public class Enemy_DataScript : ScriptableObject {

    public int Health;

    public int Speed;
    public int Acceleration;

    public int attackCooldown;
    public int attackDamage;
    public float attackRange;

    public GameObject enemiePrefab;
}
