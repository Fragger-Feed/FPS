using System.Security.Cryptography.X509Certificates;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemie_Data", menuName = "Static/Enemie_Data")]
public class Enemie_Data : ScriptableObject
{
    public int health;
    public int speed;
    public int attackCooldown;
    public int attackDamage;
    public float attackRange;

    public GameObject enemiePrefab;
}
