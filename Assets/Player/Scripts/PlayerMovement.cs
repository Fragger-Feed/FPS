using UnityEngine;
using UnityEngine.Windows;

public class PlayerMovement : MonoBehaviour
{
    // == Components ==
    [SerializeField] private Rigidbody RB;

    // == Objects ==
    [SerializeField] private GameObject Forward;

    // == Configs ==
    [SerializeField] private float speed;

    [SerializeField] private float acceleration;

    [SerializeField] public float timeToApex;

    [SerializeField] public float jumpHeight;

    // == UPDATE ==
    public void UPDATE_Movement(Vector2 InputData)
    {
        Vector3 inputDirection = Forward.transform.localRotation * new Vector3(InputData.x, 0, InputData.y);
        // Move
        Vector3 currentVelocity = RB.linearVelocity;
        Vector3 targetVelocity = inputDirection * speed;

        Vector3 newVelocity = Vector3.MoveTowards(currentVelocity, targetVelocity, acceleration);
        Vector3 finalVelocity = new Vector3(newVelocity.x, currentVelocity.y, newVelocity.z);

        RB.linearVelocity = finalVelocity;
    }


    public void Jump()
    {
        RB.linearVelocity = new Vector3(RB.linearVelocity.x, (2 * jumpHeight) / timeToApex, RB.linearVelocity.z);
    }
}