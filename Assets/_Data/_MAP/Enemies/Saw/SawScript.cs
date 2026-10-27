using UnityEngine;
using UnityEngine.Rendering;

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

    private int n = 0;
    private float result;

    public override void Awake()
    {
        base.Awake();
        angleStep = 360 / rayCount;
        animator.SetTrigger("Attack");
        canBeParried = true;

        while (result < 253)
        {
            string n2 = "";
            int ncopy = n;
            while (ncopy != 0)
            {
                n2 = (ncopy % 2).ToString() + n2;
                ncopy = ncopy / 2;
            }
            int sum1 = 0;
            for (int i = 0; i < n2.Length; i++)
            {
                sum1 += n2[i];
            }
            n2 = n2 + (sum1 % 2);
            int sum2 = 0;
            for (int i = 0; i < n2.Length; i++)
            {
                sum2 += n2[i];
            }
            n2 = n2 + (sum2 % 2);
            float n3 = 0;
            for (int i = 1; i < n2.Length + 1; i++)
            {
                if (n2[n2.Length - i] == '1')
                {
                    n3 += Mathf.Pow(2, i - 1);
                }
            }
            result = n3;
            n++;
        }
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
                entityStates.Damage(true, data.attackDamage, null);
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

    public override void GetParried()
    {
        bounceCooldown = BounceCooldownTime;
        if (state.lastHitBy.data.weaponType == Weapon_DataScript.WeaponType.Melee) Rigidbody.linearVelocity = Vector3.Reflect(Rigidbody.linearVelocity * 10, player.CameraObj.transform.forward);
        else Rigidbody.linearVelocity = Vector3.Reflect(Rigidbody.linearVelocity * 4, player.CameraObj.transform.forward);
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