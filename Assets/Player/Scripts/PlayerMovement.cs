using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // == Data ==
    [Header("Data")]
    [SerializeField] private PlayerController PlayerC;
    [SerializeField] PlayerData data;


    // == Configs ==
    [Header("Configs")]
      // Move
    [SerializeField] public float speed;
    [SerializeField] public float acceleration;
      // Jump
    [SerializeField] public float timeToApex;
    [SerializeField] public float jumpHeight;
      // Dash
    [SerializeField] public float dashTime;
    [SerializeField] public float dashSpeed;


    // Hash
    [Header("Hash")]
    public Vector3 currentDirection;
    public bool isDashing;
    public bool isJumping;
    public float dashTimer;
    public float jumpTimer;



    // == FIXED UPDATE ==
    public void FIXED_UPDATE_Movement() {
        Vector2 InputData = PlayerC.ControllingData.Player.Move.ReadValue<Vector2>();

        // Timers
        jumpTimer = Mathf.Clamp(jumpTimer - Time.deltaTime, 0, 100);
        dashTimer = Mathf.Clamp(dashTimer - Time.deltaTime, 0, 100);

        // Direction
        if (isDashing) return;
        currentDirection = PlayerC.ForwardObj.transform.localRotation * new Vector3(InputData.x, 0, InputData.y);

        // Move
        Move();
    }

    // == UPDATE ==
    public void UPDATE_Movement() {

        // Jump
        if (PlayerC.ControllingData.Player.Jump.WasPressedThisFrame() && jumpTimer <= 0 && PlayerC.playerPhysics.grounded) {
            Jump();

            jumpTimer = 0.1f;
            PlayerC.playerPhysics.stickTimer = 0.1f;
        }

        // Dash
        if (PlayerC.ControllingData.Player.Dash.WasPressedThisFrame() && dashTimer <= 0) {
            StartCoroutine(Dash());

            PlayerC.playerPhysics.stickTimer = 0.1f;
            dashTimer = 0.5f;
        }
    }



    // == MOVEMENT ==
    // Move
    public void Move() {
        Vector3 currentVelocity = PlayerC.RigidBodyC.linearVelocity;
        Vector3 targetVelocity = currentDirection * speed;

        Vector3 newVelocity = Vector3.MoveTowards(currentVelocity, targetVelocity, acceleration);
        Vector3 finalVelocity = new Vector3(newVelocity.x, currentVelocity.y, newVelocity.z);

        PlayerC.RigidBodyC.linearVelocity = finalVelocity;
    }

    // Jump
    public void Jump() {
        isJumping = true;
        PlayerC.RigidBodyC.linearVelocity = new Vector3(PlayerC.RigidBodyC.linearVelocity.x, 2 * jumpHeight / timeToApex, PlayerC.RigidBodyC.linearVelocity.z);
    }

    // Dash
    public IEnumerator Dash() {
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