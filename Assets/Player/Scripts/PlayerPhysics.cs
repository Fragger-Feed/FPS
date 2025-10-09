using UnityEngine;

public class PlayerPhysics : MonoBehaviour
{
    [SerializeField] private PlayerData data;

    [SerializeField] private Rigidbody rb;
    [SerializeField] CapsuleCollider playerCollider;
    float playerFeet;

    public void PhysicsUpdate(bool grounded, Controlling_Data input)
    {
        if (rb.linearVelocity.y > 0 && !input.Player.Jump.IsPressed() && !grounded)
            Gravity(-70);
        else if (!grounded)
            Gravity(-(2 * data.jumpHeight) / (data.timeToApex * data.timeToApex));
    }

    public void StickToTheGround(LayerMask ground)
    {
        playerFeet = playerCollider.height / 2 - playerCollider.center.y;
        RaycastHit raycastHit;
        if (Physics.Raycast(transform.position, -transform.up, out raycastHit, playerFeet + 0.5f, ground))
        {
            print(raycastHit);
        }
    }

    private void Gravity(float gravity)
    {
        //rb.AddForce(new Vector3(0, gravity, 0), ForceMode.Acceleration);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.rebeccaPurple;
        Debug.DrawRay(transform.position, -transform.up * (playerFeet + 0.1f), Color.rebeccaPurple);
    }
}