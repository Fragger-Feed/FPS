using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // == Data ==
    [Header("Data")]
    [SerializeField] private PlayerController PlayerC;

    // == Data ==
    public float speed;

    public float acceleration;
    public float timeToApex;
    public float jumpHeight;
    public float dashTime;
    public float dashSpeed;

    // Hash
    [Header("Hash")]
    public Vector3 currentDirection;

    public bool isDashing;
    public bool isJumping;
    public float dashTimer;
    public float jumpTimer;
    public float jumpBufferTimer;
    public Vector2 savedInput;
    public bool disableMovement;

    private void Awake()
    {
        speed = PlayerC.data.speed;
        acceleration = PlayerC.data.acceleration;
        timeToApex = PlayerC.data.timeToApex;
        jumpHeight = PlayerC.data.jumpHeight;
        dashTime = PlayerC.data.dashTime;
        dashSpeed = PlayerC.data.dashSpeed;
    }

    // === UPDATE ===
    // = Fixed Update =
    public void FIXED_UPDATE_Movement()
    {
        // Timers
        jumpTimer = Mathf.Clamp(jumpTimer - Time.deltaTime, 0, 100);
        dashTimer = Mathf.Clamp(dashTimer - Time.deltaTime, 0, 100);

        // Direction
        if (isDashing || disableMovement) return;
        currentDirection = PlayerC.ForwardObj.transform.localRotation * new Vector3(savedInput.x, 0, savedInput.y);

        // Move
        Move();
    }

    // = Update =
    public void UPDATE_Movement()
    {
        jumpBufferTimer -= Time.deltaTime;
        savedInput = PlayerC.ControllingData.Player.Move.ReadValue<Vector2>();

        // Jump
        if (PlayerC.ControllingData.Player.Jump.WasPressedThisFrame() && jumpTimer <= 0)
        {
            jumpBufferTimer = 0.2f;
            jumpTimer = 0.1f;
        }

        // Dash
        if (PlayerC.ControllingData.Player.Dash.WasPressedThisFrame() && dashTimer <= 0)
        {
            StartCoroutine(Dash());

            PlayerC.playerPhysics.stickTimer = 0.1f;
            dashTimer = 0.5f;
        }

        if (jumpBufferTimer > 0 && PlayerC.playerPhysics.coyoteTime > 0 && !isJumping)
        {
            Jump();
        }
    }

    // == MOVEMENT ==
    // Move
    public void Move()
    {
        Vector3 velocity = PlayerC.RigidBodyC.linearVelocity;
        Vector3 targetVelocity = currentDirection * speed;
        Vector3 newVelocity = Vector3.MoveTowards(new Vector3(velocity.x, 0f, velocity.z), new Vector3(targetVelocity.x, 0f, targetVelocity.z), acceleration * Time.deltaTime);

        velocity.x = newVelocity.x;
        velocity.z = newVelocity.z;

        PlayerC.RigidBodyC.linearVelocity = velocity;
    }

    // Jump
    public void Jump()
    {
        isJumping = true;
        PlayerC.RigidBodyC.linearVelocity = new Vector3(PlayerC.RigidBodyC.linearVelocity.x, 2 * jumpHeight / timeToApex, PlayerC.RigidBodyC.linearVelocity.z);
        PlayerC.playerPhysics.stickTimer = 0.1f;
    }

    // Dash
    public IEnumerator Dash()
    {
        if (currentDirection == Vector3.zero) currentDirection = PlayerC.ForwardObj.transform.localRotation * new Vector3(0, 0, 1);
        float dashDuration = dashTime;
        isDashing = true;
        while (dashDuration > 0)
        {
            dashDuration -= Time.deltaTime;
            PlayerC.RigidBodyC.linearVelocity = currentDirection * dashSpeed;
            yield return null;
        }
        isDashing = false;
    }
}