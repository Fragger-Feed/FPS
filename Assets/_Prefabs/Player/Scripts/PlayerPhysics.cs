using UnityEngine;
using UnityEngine.Localization.SmartFormat.Core.Parsing;
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

    // Hash
    [Header("Hash")]
    public Vector3 gcSize;

    public Vector3 gcOffset;
    public Vector3 gscSize;
    public Vector3 gscOffset;
    public float slopCheckDistance = 0.5f;
    public LayerMask ground;
    public LayerMask enemieAndGround;
    public float stickTimer;
    public bool grounded;
    public bool groundedSlop;
    private float playerFeet;
    public float coyoteTime = 0.1f;
    public Vector3 groundNormal;

    // === UPDATE ===
    // = Update =
    public void UPDATE_Physics()
    {
        if (!PlayerC.playerMovement.disableMovement)
        {
            coyoteTime -= Time.deltaTime;
            stickTimer -= Time.deltaTime;
            groundCheck();
            slopeCheck();
            if (stickTimer <= 0) { StickToTheGround(ground); }
        }
    }

    // = Fixed Update =
    public void FIXED_UPDATE_Physics()
    {
        float gravity = -(2 * PlayerC.playerMovement.jumpHeight) / (PlayerC.playerMovement.timeToApex * PlayerC.playerMovement.timeToApex);
        if (PlayerC.RigidBodyC.linearVelocity.y > 0 && !PlayerC.ControllingData.Player.Jump.IsPressed() && coyoteTime < 0)
            Gravity(gravity * 2);
        else if (!grounded)
            Gravity(gravity);
    }

    // === PHYSICS
    // To Ground
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

    // Gravity
    private void Gravity(float gravity)
    {
        GravityValue = gravity;
        PlayerC.RigidBodyC.AddForce(new Vector3(0, gravity, 0), ForceMode.Acceleration);
    }

    // === CHECKS ====
    // Ground
    private void groundCheck()
    {
        if (Physics.CheckBox(transform.position + gcOffset, gcSize, new Quaternion(0, 0, 0, 0), enemieAndGround))
        {
            coyoteTime = 0.2f;
            PlayerC.playerMovement.isJumping = false;
            grounded = true;
        }
        else grounded = false;
    }

    // Slope
    public void slopeCheck()
    {
        groundedSlop = false;
        RaycastHit hit;
        Vector3 origin = transform.position + gscOffset;

        if (Physics.BoxCast(origin, gscSize, -transform.up, out hit, transform.rotation, slopCheckDistance, ground, QueryTriggerInteraction.Ignore))
        {
            float angle = Vector3.Angle(hit.normal, Vector3.up);
            if (angle <= 70f && angle > 0)
            {
                groundedSlop = true;
                groundNormal = hit.normal;
            }
            Debug.DrawRay(hit.point, hit.normal, Color.green);
        }
        Debug.DrawRay(origin, -transform.up * 1.2f, Color.yellow);
    }

    // === DRAW ===
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + gcOffset, gcSize * 2);
        Gizmos.color = Color.rebeccaPurple;
        Gizmos.DrawWireCube(transform.position + gscOffset, gscSize * 2);
        Gizmos.DrawWireCube(transform.position - transform.up * slopCheckDistance + gscOffset, gscSize * 2);

        if (PlayerC.playerCollider == null) return;

        float num = (PlayerC.playerCollider.height / 2f - PlayerC.playerCollider.center.y) * transform.lossyScale.y;
        Vector3 bottom = transform.position - transform.up * num;
        Vector3 top = transform.position + transform.up * (PlayerC.playerCollider.height / 2f + PlayerC.playerCollider.center.y);

        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(bottom, top);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(bottom, PlayerC.playerCollider.radius);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 0.05f);
    }
}