using System;
using UnityEngine;
using static Enemy_Script;

public class EntityStates : MonoBehaviour
{
    private int maxHealth;
    [SerializeField] private int currentHealth;

    private Enemy_Script enemyScript;
    private Enemy_DataScript enemieData;

    private void Awake()
    {
        enemyScript = GetComponent<Enemy_Script>();
        enemieData = enemyScript.data;

        maxHealth = enemieData.Health;
        currentHealth = maxHealth;
    }

    public void Heal(int heal)
    { currentHealth = Mathf.Min(currentHealth + heal, maxHealth); }

    public void Damage(int damage)
    {
        print($"took {damage} damage");
        if (damage == 0)
        {
            print("BUG");
        }
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            enemyScript.currentState = EnemieState.Dead;
            Die();
        }
    }

    public void Die()
    {
        print($"I'm object {gameObject} and I'm dead");
        enemyScript.nav.enabled = false;
        Rigidbody rb = gameObject.AddComponent<Rigidbody>();
        if (rb != null)
            rb.mass = 112;
        transform.Rotate(90, 90, 90);
    }
}