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

    private void Start()
    {
    }

    // == UPDATE ==
    public void UPDATE_Movement(Vector2 InputData)
    {
        // Move
        Vector3 currentVelocity = RB.linearVelocity;
        Vector3 targetVelocity = new Vector3(InputData.x, 0, InputData.y) * speed;

        Vector3 newVelocity = Vector3.MoveTowards(currentVelocity, targetVelocity, acceleration);
        Vector3 finalVelocity = new Vector3(newVelocity.x, currentVelocity.y, newVelocity.z);

        RB.linearVelocity = finalVelocity;
    }
}