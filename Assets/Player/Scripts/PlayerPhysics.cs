using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.UIElements;

public class PlayerPhysics : MonoBehaviour
{
    [SerializeField] private PlayerData data;

    [SerializeField] private Rigidbody rb;
    [SerializeField] CapsuleCollider playerCollider;


    float playerFeet;

    public void PhysicsUpdate(bool grounded, Controlling_Data input)
    {
        float gravity = -(2 * data.jumpHeight) / (data.timeToApex * data.timeToApex);
        if (rb.linearVelocity.y > 0 && !input.Player.Jump.IsPressed() && !grounded)
            Gravity(gravity*2);
        else if (!grounded)
            Gravity(gravity);
    }

    public void StickToTheGround(LayerMask ground)
    {
        playerFeet = (playerCollider.height / 2 - playerCollider.center.y) * transform.lossyScale.y;
        if (rb.linearVelocity != Vector3.zero)
        {
            RaycastHit raycastHit;
            if (Physics.Raycast(transform.position, -transform.up, out raycastHit, playerFeet + 0.1f, ground))
            {
                Vector3 targetPos = new Vector3(transform.position.x, transform.position.y - raycastHit.distance + playerFeet, transform.position.z);
                transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * 10);

                //if (rb.linearVelocity.y > 0) rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
                print(playerFeet + " Num");
            }
        }
        if (rb.linearVelocity.x == 0 && rb.linearVelocity.z == 0)
        {
            RaycastHit raycastHit;
            if (Physics.Raycast(transform.position, -transform.up, out raycastHit, playerFeet + 0.1f, ground))
                rb.linearVelocity = Vector3.zero;
        }


    }

    private void Gravity(float gravity)
    {
        data.sdfghjuygfyvubuhabfuabfuhasbhab = gravity;
        rb.AddForce(new Vector3(0, gravity, 0), ForceMode.Acceleration);
    }

    void OnDrawGizmos()
{
    if (playerCollider == null) return;

    float num = (playerCollider.height / 2f - playerCollider.center.y) * transform.lossyScale.y;
    Vector3 bottom = transform.position - transform.up * num;
    Vector3 top = transform.position + transform.up * (playerCollider.height / 2f + playerCollider.center.y);

    // draw collider axis
    Gizmos.color = Color.cyan;
    Gizmos.DrawLine(bottom, top);

    // draw bottom sphere
    Gizmos.color = Color.green;
    Gizmos.DrawWireSphere(bottom, playerCollider.radius);

    // draw center
    Gizmos.color = Color.yellow;
    Gizmos.DrawWireSphere(transform.position, 0.05f);
}

}