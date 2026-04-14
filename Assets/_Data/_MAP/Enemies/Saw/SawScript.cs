using UnityEngine;

public class SawScript : Enemy_Script
{
    public GameObject Player;
    public Rigidbody Rigidbody;

    [SerializeField, Range(0, 1)] private float playerDirectionLerp;

    public override void Awake()
    {
        base.Awake();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        if (currentState != EnemieState.Move) return;

        if (Rigidbody.linearVelocity == Vector3.zero)
            Rigidbody.linearVelocity = Vector3.forward * data.Speed;

        Vector3 playerDirection = (player.transform.position - transform.position).normalized;
        Vector3 currentDirection = Rigidbody.linearVelocity.normalized;
        Vector3 finalDirection = Vector3.Lerp(currentDirection, playerDirection, playerDirectionLerp).normalized;

        Rigidbody.AddForce(finalDirection * data.Acceleration);

        Rigidbody.linearVelocity = finalDirection * data.Speed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Rigidbody.linearVelocity = Vector3.Reflect(Rigidbody.linearVelocity, collision.contacts[0].normal);
    }
}