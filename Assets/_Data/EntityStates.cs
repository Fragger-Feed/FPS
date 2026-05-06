using System;
using UnityEngine;
using static Enemy_Script;

public class EntityStates : MonoBehaviour
{
    private int maxHealth;
    [SerializeField] private int currentHealth;

    public bool IsDead => currentHealth <= 0;

    public Enemy_Script enemyScript;
    private Enemy_DataScript enemyData;

    public WeaponScript lastHitBy;

    private void Awake()
    {
        enemyScript = GetComponent<Enemy_Script>();
        enemyData = enemyScript.data;

        maxHealth = enemyData.Health;
        currentHealth = maxHealth;
    }

    public void Heal(int heal)
    { currentHealth = Mathf.Min(currentHealth + heal, maxHealth); }

    public void Damage(bool melee, int damage, WeaponScript weapon)
    {
        print($"took {damage} damage");
        if (enemyScript != null && melee && enemyScript.canBeParried)
        {
            GetParried();
        }
        if (damage == 0)
        {
            print("BUG");
        }
        currentHealth -= damage;
        if (weapon != null)
            lastHitBy = weapon;
        if (currentHealth <= 0)
        {
            enemyScript.currentState = EnemyState.Dead;
            Die();
        }
    }

    public void GetParried()
    {
        enemyScript.stunTimer = enemyData.stunTime;
        enemyScript.currentState = enemyScript.currentState = EnemyState.Stunned;
        enemyScript.canBeParried = false;
        enemyScript.attacking = false;
        enemyScript.animator.SetBool("Stunned", true);
        print("PPPPPPARY");
    }

    public void Die()
    {
        Animator anim = GetComponent<Animator>();
        anim.StopPlayback();
        print($"I'm object {gameObject} and I'm dead");
        if (enemyScript.nav != null)
            enemyScript.nav.enabled = false;
        gameObject.AddComponent<Rigidbody>();
        Rigidbody rb = gameObject.AddComponent<Rigidbody>();
        if (rb != null)
            rb.mass = 112;
        transform.Rotate(90, 90, 90);
    }
}