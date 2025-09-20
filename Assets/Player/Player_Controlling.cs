using System;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // == Player Scripts ==
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerView playerView;

    // == Data ==
    [SerializeField] private Controlling_Data ControllingData;

    [SerializeField] private Vector3 gcSize;

    [SerializeField] private Vector3 gcoffset;

    [SerializeField] private LayerMask ground;

    public bool grounded;

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
        groundCheck();
        playerView.UPDATE_View(ControllingData.Player.Look.ReadValue<Vector2>(), transform.position);
        if (ControllingData.Player.Jump.WasPressedThisFrame() && grounded) playerMovement.Jump();
    }

    private void FixedUpdate()
    {
        playerMovement.UPDATE_Movement(ControllingData.Player.Move.ReadValue<Vector2>());
        if (!grounded)
            playerMovement.Gravity();
    }

    private void groundCheck()
    {
        if (Physics.CheckBox(transform.position + gcoffset, gcSize, new Quaternion(0, 0, 0, 0), ground))
        {
            grounded = true;
        }
        else grounded = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + gcoffset, gcSize * 2);
    }
}