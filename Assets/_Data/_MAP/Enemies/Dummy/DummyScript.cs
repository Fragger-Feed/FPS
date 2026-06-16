using UnityEngine;

public class DummyScript : Enemy_Script
{
    public override void Awake()
    {
        base.Awake();
        nav.stoppingDistance = data.attackRange;
    }

    public override void Move()
    {
        base.Move();

        if (Vector3.Distance(transform.position, player.transform.position) <= data.attackRange)
        {
            currentState = EnemyState.Attack;
            return;
        }

        animator.SetTrigger("Move");
        animator.ResetTrigger("Attack");

        nav.SetDestination(player.transform.position);
    }

    public override void Attack()
    {
        base.Attack();

        animator.ResetTrigger("Move");
        animator.SetTrigger("Attack");

        if (Vector3.Distance(transform.position, player.transform.position) >= data.attackRange && canMove)
        {
            currentState = EnemyState.Move;
            return;
        }

        if (attacking && !alreadyDamaged)
        {
            Damage();
        }

        /*        if (Time.time < cooldownTimer)
                    return;*/
        cooldownTimer = Time.time + data.attackCooldown;
    }

    public override void GetParried()
    {
        stunTimer = data.stunTime;
        currentState = currentState = EnemyState.Stunned;
        canBeParried = false;
        attacking = false;
        animator.SetBool("Stunned", true);
        print("PPPPPPARY");
    }

    public void Damage()
    {
        if (alreadyDamaged) return;
        Collider[] hits = Physics.OverlapSphere(transform.position, data.attackRange, LayerMask.GetMask("Player"));
        foreach (var hit in hits)
        {
            print(hit);
            PlayerState player = hit.GetComponent<PlayerState>();
            if (player != null)
            {
                player.TakeDamage(data.attackDamage);
                alreadyDamaged = true;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, data.attackRange);
    }
}