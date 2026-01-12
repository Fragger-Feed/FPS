using UnityEngine;

public class DummyScript : EnemieScript
{
    public override void Move()
    {
        base.Move();

        nav.SetDestination(player.transform.position);
    }

    public override void Attack()
    {
        base.Attack();

        if (Time.time < data.attackCooldown)
            return;

        Collider[] hits = Physics.OverlapSphere(transform.position, data.attackRange, 8);
        foreach (var hit in hits)
        {
            PlayerState player = hit.GetComponent<PlayerState>();
            if (player != null)
            {
                player.TakeDamage(data.attackDamage);
            }
        }

        cooldownTimer = Time.time + data.attackCooldown;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, data.attackRange);
    }
}