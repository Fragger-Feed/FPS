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
            currentState = EnemieState.Attack;
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

        if (attacking)
        {
            Damage();
        }

        /*        if (Time.time < cooldownTimer)
                    return;*/
        cooldownTimer = Time.time + data.attackCooldown;
    }

    public void Damage()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, data.attackRange, LayerMask.GetMask("Player"));
        foreach (var hit in hits)
        {
            print(hit);
            PlayerState player = hit.GetComponent<PlayerState>();
            if (player != null)
            {
                player.TakeDamage(data.attackDamage);
            }
        }
        currentState = EnemieState.Move;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, data.attackRange);
    }
}