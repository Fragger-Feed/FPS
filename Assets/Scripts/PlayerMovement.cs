using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float speed;
    [SerializeField] private float acceleration;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Movement(Vector3 input)
    {
        Vector3 currentVelocity = rb.linearVelocity;
        Vector3 targetVelocity = new Vector3(input.x, 0, input.y) * speed;
        
        Vector3 newVelocity = Vector3.MoveTowards(currentVelocity, targetVelocity, acceleration);
        Vector3 finalVelocity = new Vector3(newVelocity.x, currentVelocity.y, newVelocity.z);
        
        rb.linearVelocity = finalVelocity;
    }
}
