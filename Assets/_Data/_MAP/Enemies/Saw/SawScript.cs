using UnityEngine;

public class SawScript : Enemy_Script
{
    public Rigidbody Rigidbody;

    [SerializeField, Range(0, 1)] private float playerDirectionLerp;

    [SerializeField] private int rayCount = 8;
    [SerializeField] private float distance;

    [SerializeField] private LayerMask excludeLayer;

    private float bounceCooldown;
    private const float BounceCooldownTime = 0.1f;
    private float angleStep;

    public override void Awake()
    {
        base.Awake();
        angleStep = 360 / rayCount;
        animator.SetTrigger("Attack");
    }

    public override void Move()
    {
        base.Move();
        currentState = EnemyState.Attack;
    }

    public override void Attack()
    {
        base.Attack();

        if (Time.time < cooldownTimer) return;

        Collider[] hits = Physics.OverlapBox(transform.position, new Vector3(data.attackRange, data.attackRangeY, data.attackRange));
        foreach (Collider hit in hits)
        {
            PlayerState playerState = hit.GetComponent<PlayerState>();
            EntityStates entityStates = hit.GetComponent<EntityStates>();

            if (playerState != null)
            {
                playerState.TakeDamage(data.attackDamage);
            }
            if (entityStates != null)
            {
                entityStates.Damage(true, data.attackDamage);
            }
        }

        cooldownTimer = Time.time + data.attackCooldown;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        bounceCooldown -= Time.fixedDeltaTime;

        bool bounced = false;

        if (currentState != EnemyState.Attack) return;

        if (Rigidbody.linearVelocity == Vector3.zero)
            Rigidbody.linearVelocity = Vector3.forward * data.Speed;

        if (bounceCooldown <= 0)
        {
            for (int i = 0; i < rayCount; i++)
            {
                float angle = i * angleStep;
                Vector3 dir = Quaternion.Euler(0, angle, 0) * Vector3.forward;

                if (Physics.Raycast(transform.position, dir, out RaycastHit hit, distance/*, ~excludeLayer*/))
                {
                    Debug.DrawRay(transform.position, dir, Color.red);
                    print(Rigidbody.linearVelocity);
                    bounceCooldown = BounceCooldownTime;
                    Rigidbody.linearVelocity = Vector3.Reflect(Rigidbody.linearVelocity, hit.normal);
                    bounced = true;
                    break;
                }
            }
        }

        if (bounced) return;

        Vector3 playerDirection = (player.transform.position - transform.position).normalized;
        Vector3 currentDirection = Rigidbody.linearVelocity.normalized;
        Vector3 lerpDirection = Vector3.Lerp(currentDirection, playerDirection, playerDirectionLerp).normalized;

        Vector3 finalDirection = lerpDirection * data.Speed;
        finalDirection.y = Rigidbody.linearVelocity.y;

        Rigidbody.linearVelocity = finalDirection;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.orange;

        for (int i = 0; i < rayCount; i++)
        {
            float angle = i * angleStep;
            Vector3 dir = Quaternion.Euler(0, angle, 0) * Vector3.forward;

            Gizmos.DrawRay(transform.position, dir * distance);
        }
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(data.attackRange * 2, data.attackRangeY * 2, data.attackRange * 2));
    }
}