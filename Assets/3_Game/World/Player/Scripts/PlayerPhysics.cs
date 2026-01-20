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
    [SerializeField] private Vector3 gcOffset;
    [SerializeField] private Vector3 gscSize;
    [SerializeField] private Vector3 gscOffset;
    [SerializeField] private float slopCheckDistance = 0.5f;
    [SerializeField] private LayerMask ground;
    [SerializeField] private LayerMask enemieAndGround;

    // Hash
    [Header("Hash")]
    public float stickTimer;

    public bool grounded;
    public bool groundedSlop;

    private float playerFeet;
    public float coyoteTime = 0.1f;

    public void UPDATE_Physics()
    {
        coyoteTime -= Time.deltaTime;
        stickTimer -= Time.deltaTime;
        groundCheck();
        slopeCheck();
        print(PlayerC.RigidBodyC.linearVelocity);
        if (stickTimer <= 0)
        {
            StickToTheGround(ground);
        }
    }

    public void FIXED_UPDATE_Physics()
    {
        float gravity = -(2 * PlayerC.playerMovement.jumpHeight) / (PlayerC.playerMovement.timeToApex * PlayerC.playerMovement.timeToApex);
        if (PlayerC.RigidBodyC.linearVelocity.y > 0 && !PlayerC.ControllingData.Player.Jump.IsPressed() && coyoteTime < 0)
            Gravity(gravity * 2);
        else if (grounded && PlayerC.ControllingData.Player.Move.ReadValue<Vector2>() == Vector2.zero)
            Gravity(0);
        else if (!grounded)
            Gravity(gravity);
    }

    private void groundCheck()
    {
        if (Physics.CheckBox(transform.position + gcOffset, gcSize, new Quaternion(0, 0, 0, 0), enemieAndGround))
        {
            coyoteTime = 0.2f;
            PlayerC.playerMovement.isJumping = false;
            grounded = true;
        }
        else
            grounded = false;
    }

    public void slopeCheck()
    {
        groundedSlop = false;

        RaycastHit hit;
        Vector3 origin = transform.position + gscOffset;

        if (Physics.BoxCast(
            origin,
            gscSize,
            -transform.up,
            out hit,
            transform.rotation,
            1.2f,
            ground,
            QueryTriggerInteraction.Ignore))
        {
            float angle = Vector3.Angle(hit.normal, Vector3.up);
            /*            print(angle);*/
            if (angle <= 70f && angle > 0)
            {
                groundedSlop = true;
            }

            Debug.DrawRay(hit.point, hit.normal, Color.green);
        }

        Debug.DrawRay(origin, -transform.up * 1.2f, Color.yellow);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + gcOffset, gcSize * 2);
        Gizmos.color = Color.rebeccaPurple;
        Gizmos.DrawWireCube(transform.position + gscOffset, gscSize * 2);

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
        if (PlayerC.RigidBodyC.linearVelocity != Vector3.zero && !groundedSlop)
        {
            RaycastHit raycastHit;
            if (Physics.Raycast(transform.position, -transform.up, out raycastHit, playerFeet + 1, ground))
            {
                Vector3 targetPos = new Vector3(transform.position.x, transform.position.y - raycastHit.distance + playerFeet, transform.position.z);
                transform.position = Vector3.MoveTowards(transform.position, targetPos, raycastHit.distance * Time.deltaTime * 10);
                if (PlayerC.RigidBodyC.linearVelocity.y > 0 && !PlayerC.playerMovement.isJumping && !PlayerC.playerMovement.isDashing && grounded) PlayerC.RigidBodyC.linearVelocity = new Vector3(PlayerC.RigidBodyC.linearVelocity.x, 0f, PlayerC.RigidBodyC.linearVelocity.z);
            }
        }
        if (grounded && PlayerC.ControllingData.Player.Move.ReadValue<Vector2>() == Vector2.zero && !PlayerC.playerMovement.isJumping)
        {
            PlayerC.RigidBodyC.linearVelocity = new Vector3(PlayerC.RigidBodyC.linearVelocity.x, 0f, PlayerC.RigidBodyC.linearVelocity.z);
        }
    }

    /*
        public void StickToTheGround(LayerMask ground)
    {
        float num = (PlayerC.playerCollider.height / 2f - PlayerC.playerCollider.center.y) * transform.lossyScale.y;
        if (PlayerC.RigidBodyC.linearVelocity != Vector3.zero && Physics.Raycast(transform.position, -transform.up, out var hitInfo3, num + 1f, ground))
        {
            Vector3 target = new Vector3(transform.position.x, transform.position.y - hitInfo3.distance + num, transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, target, hitInfo3.distance * Time.deltaTime * 10f);
            if (PlayerC.RigidBodyC.linearVelocity.y > 0f)
                PlayerC.RigidBodyC.linearVelocity = new Vector3(PlayerC.RigidBodyC.linearVelocity.x, 0f, PlayerC.RigidBodyC.linearVelocity.z);
        }
    }
    */

    private void Gravity(float gravity)
    {
        GravityValue = gravity;
        PlayerC.RigidBodyC.AddForce(new Vector3(0, gravity, 0), ForceMode.Acceleration);
    }
}