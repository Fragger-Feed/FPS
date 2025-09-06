using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Movement(Vector3 input)
    {
        Vector3 currentVelocity = rb.linearVelocity;
        
        Vector3 newVelocity = new Vector3(input.x, currentVelocity.y, input.y) * 10;
        rb.linearVelocity = newVelocity;
        print(rb.linearVelocity);
    }
}
