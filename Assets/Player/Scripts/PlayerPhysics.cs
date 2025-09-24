using UnityEngine;

public class PlayerPhysics : MonoBehaviour
{
    [SerializeField] private PlayerData data;

    [SerializeField] private Rigidbody rb;

    public void PhysicsUpdate(bool grounded, Controlling_Data input)
    {
        if (rb.linearVelocity.y > 0 && !input.Player.Jump.IsPressed() && !grounded)
            Gravity(-70);
        else if (!grounded)
            Gravity(-(2 * data.jumpHeight) / (data.timeToApex * data.timeToApex));
    }

    private void Gravity(float gravity)
    {
        rb.AddForce(new Vector3(0, gravity, 0), ForceMode.Acceleration);
    }
}