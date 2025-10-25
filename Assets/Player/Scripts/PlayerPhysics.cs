using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.UIElements;

public class PlayerPhysics : MonoBehaviour
{
    // == Data ==
    [Header("Data")]
    [SerializeField] private PlayerController PlayerC;

    // == Configs ==
    [Header("Configs")]
    public float GravityValue;
    [SerializeField] private Vector3 gcSize;
    [SerializeField] private Vector3 gcoffset;
    [SerializeField] private LayerMask ground;


    // Hash
    [Header("Hash")]
    public float stickTimer;
    public bool grounded;






    float playerFeet;



    public void UPDATE_Physics()
    {
        stickTimer -= Time.deltaTime;
        groundCheck();

    }
    public void FIXED_UPDATE_Physics()
    {
        float gravity = -(2 * PlayerC.playerMovement.jumpHeight) / (PlayerC.playerMovement.timeToApex * PlayerC.playerMovement.timeToApex);
        if (PlayerC.RigidBodyC.linearVelocity.y > 0 && !PlayerC.ControllingData.Player.Jump.IsPressed() && !grounded)
            Gravity(gravity*2);
        else if (!grounded)
            Gravity(gravity);


        if (stickTimer <= 0)
        {
            StickToTheGround(ground);
        }
    }














    private void groundCheck()
    {
        if (Physics.CheckBox(transform.position + gcoffset, gcSize, new Quaternion(0, 0, 0, 0), ground))
        {
            grounded = true;
            PlayerC.playerMovement.isJumping = false;
        }

        else grounded = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + gcoffset, gcSize * 2);


        if (PlayerC.playerCollider == null) return;

        float num = (PlayerC.playerCollider.height / 2f - PlayerC.playerCollider.center.y) * transform.lossyScale.y;
        Vector3 bottom = transform.position - transform.up * num;
        Vector3 top = transform.position + transform.up * (PlayerC.playerCollider.height / 2f + PlayerC.playerCollider.center.y);

        // draw collider axis
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(bottom, top);

        // draw bottom sphere
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(bottom, PlayerC.playerCollider.radius);

        // draw center
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 0.05f);
    }




    public void StickToTheGround(LayerMask ground)
    {
        playerFeet = (PlayerC.playerCollider.height / 2 - PlayerC.playerCollider.center.y) * transform.lossyScale.y;
        if (PlayerC.RigidBodyC.linearVelocity != Vector3.zero)
        {
            RaycastHit raycastHit;
            if (Physics.Raycast(transform.position, -transform.up, out raycastHit, playerFeet + 1f, ground))
            {
                Vector3 targetPos = new Vector3(transform.position.x, transform.position.y - raycastHit.distance + playerFeet, transform.position.z);
                transform.position = Vector3.MoveTowards(transform.position, targetPos,raycastHit.distance * Time.deltaTime * 10);

                //if (rb.linearVelocity.y > 0) rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
            }
        }
        if (PlayerC.RigidBodyC.linearVelocity.x == 0 && PlayerC.RigidBodyC.linearVelocity.z == 0)
        {
            RaycastHit raycastHit;
            if (Physics.Raycast(transform.position, -transform.up, out raycastHit, playerFeet + 1f, ground))
                PlayerC.RigidBodyC.linearVelocity = Vector3.zero;
        }


    }

    private void Gravity(float gravity)
    {
        GravityValue = gravity;
        PlayerC.RigidBodyC.AddForce(new Vector3(0, gravity, 0), ForceMode.Acceleration);
    }

    
}