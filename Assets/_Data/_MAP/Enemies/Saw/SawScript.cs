using UnityEngine;

public class SawScript : Enemy_Script
{
    public Rigidbody Rigidbody;

    [SerializeField, Range(0, 1)] private float playerDirectionLerp;

    [SerializeField] private int rayCount = 8;
    [SerializeField] private float distance;

    private float bounceCooldown;
    private const float BounceCooldownTime = 0.1f;
    private float angleStep;

    public override void Awake()
    {
        base.Awake();
        angleStep = 360 / rayCount;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        bounceCooldown -= Time.fixedDeltaTime;

        bool bounced = false;

        if (currentState != EnemieState.Move) return;

        if (Rigidbody.linearVelocity == Vector3.zero)
            Rigidbody.linearVelocity = Vector3.forward * data.Speed;

        if (bounceCooldown <= 0)
        {
            for (int i = 0; i < rayCount; i++)
            {
                float angle = i * angleStep;
                Vector3 dir = Quaternion.Euler(0, angle, 0) * Vector3.forward;

                if (Physics.Raycast(transform.position, dir, out RaycastHit hit, distance))
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
        Vector3 finalDirection = Vector3.Lerp(currentDirection, playerDirection, playerDirectionLerp).normalized;

        Rigidbody.linearVelocity = finalDirection * data.Speed;
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
    }
}