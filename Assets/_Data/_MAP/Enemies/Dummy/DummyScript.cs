using UnityEngine;

public class DummyScript : Enemy_Script
{
    public override void Move()
    {
        base.Move();

        nav.SetDestination(player.transform.position);
    }

    public override void Attack()
    {
        base.Attack();

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