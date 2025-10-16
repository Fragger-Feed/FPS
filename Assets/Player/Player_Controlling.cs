using System;
using System.Data.Common;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // == Player Scripts ==
    [SerializeField] private PlayerMovement playerMovement;

    [SerializeField] private PlayerView playerView;

    [SerializeField] private PlayerPhysics physics;

    // == Data ==
    [SerializeField] private Controlling_Data ControllingData;

    [SerializeField] private Vector3 gcSize;

    [SerializeField] private Vector3 gcoffset;

    [SerializeField] private LayerMask ground;

    public bool grounded;

    //Timers

    float dashTimer;
    float jumpTimer;

    float stickTimer;

    //Input

    Vector2 movementInput;

    private void Awake()
    {
        ControllingData = new Controlling_Data();
    }

    private void OnEnable()
    {
        ControllingData.Enable();
    }

    private void Update()
    {
        jumpTimer -= Time.deltaTime;
        dashTimer -= Time.deltaTime;
        stickTimer -= Time.deltaTime;

        movementInput = ControllingData.Player.Move.ReadValue<Vector2>();

        groundCheck();
        playerView.UPDATE_View(ControllingData.Player.Look.ReadValue<Vector2>(), transform.position);
        if (ControllingData.Player.Jump.WasPressedThisFrame() && grounded && jumpTimer < 0)
        {
            playerMovement.Jump();
            jumpTimer = 0.1f;
            stickTimer = 0.1f;
        }
        if (ControllingData.Player.Dash.WasPressedThisFrame() && dashTimer < 0)
        {
            StartCoroutine(playerMovement.Dash(movementInput));
            stickTimer = 0.1f;
            dashTimer = 0f;
        }
        
    }

    private void FixedUpdate()
    {
        playerMovement.UPDATE_Movement(movementInput);
        if (stickTimer <= 0)
        {
            physics.StickToTheGround(ground);
        } 
        physics.PhysicsUpdate(grounded, ControllingData);
    }

    private void groundCheck()
    {
        if (Physics.CheckBox(transform.position + gcoffset, gcSize, new Quaternion(0, 0, 0, 0), ground))
        {
            grounded = true;
            playerMovement.isJumping = false;
        }

        else grounded = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + gcoffset, gcSize * 2);
    }
}