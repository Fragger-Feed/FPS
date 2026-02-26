using System;
using UnityEngine;
using static Enemy_Script;

public class EntityStates : MonoBehaviour
{
    private int maxHealth;
    [SerializeField] private int currentHealth;

    private Enemy_Script enemieScript;
    private Enemie_Data enemieData;

    private void Awake()
    {
        enemieScript = GetComponent<Enemy_Script>();
        enemieData = enemieScript.data;

        maxHealth = enemieData.health;
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
            enemieScript.currentState = EnemieState.Dead;
            Die();
        }
    }

    public void Die()
    {
        print($"I'm object {gameObject} and I'm dead");
        enemieScript.nav.enabled = false;
        Rigidbody rb = gameObject.AddComponent<Rigidbody>();
        rb.mass = 112;
        transform.Rotate(90, 90, 90);
    }
}