using UnityEngine;

public class SawScript : Enemy_Script
{
    public GameObject Player;
    public Rigidbody Rigidbody;

    [SerializeField, Range(0, 1)] private float playerDirectionLerp;

    [SerializeField] private int rayCount = 8;
    [SerializeField] private float distance;
    private float angleStep;

    public override void Awake()
    {
        base.Awake();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        angleStep = 360 / rayCount;
        bool bounced = false;

        if (currentState != EnemieState.Move) return;

        if (Rigidbody.linearVelocity == Vector3.zero)
            Rigidbody.linearVelocity = Vector3.forward * data.Speed;

        for (int i = 0; i < rayCount; i++)
        {
            float angle = i * angleStep;
            Vector3 dir = Quaternion.Euler(0, angle, 0) * Vector3.forward;

            if (Physics.Raycast(transform.position, dir, out RaycastHit hit, distance))
            {
                Rigidbody.linearVelocity = Vector3.Reflect(Rigidbody.linearVelocity, hit.normal);
                bounced = true;
                break;
            }
        }

        if (bounced) return;

        Vector3 playerDirection = (player.transform.position - transform.position).normalized;
        Vector3 currentDirection = Rigidbody.linearVelocity.normalized;
        Vector3 finalDirection = Vector3.Lerp(currentDirection, playerDirection, playerDirectionLerp).normalized;

        Rigidbody.AddForce(finalDirection * data.Acceleration);

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