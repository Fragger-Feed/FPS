using System;
using System.Data.Common;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // == Data ==
    [Header("Data")]
    public Controlling_Data ControllingData;

    public Player_Data data;

    // == Player Scripts ==
    [Header("Player Scripts")]
    public PlayerMovement playerMovement;

    public PlayerView playerView;
    public PlayerPhysics playerPhysics;

    public PlayerInventory playerInventory;
    public PlayerState playerState;

    public PlayerInputController playerInputController;
    public PlayerUIController playerUIController;

    // == Components ==
    [Header("Components")]
    public Rigidbody RigidBodyC;

    public CapsuleCollider playerCollider;

    // == Objects ==
    [Header("Objects")]
    public GameObject ForwardObj;

    public Camera CameraObj;

    // === START ===
    private void Awake()
    { ControllingData = new Controlling_Data(); }

    private void OnEnable()
    { ControllingData.Enable(); }

    private void OnDisable()
    { ControllingData.Disable(); }

    // === UPDATE ===
    // = Update =
    private void Update()
    {
        playerView.UPDATE_View();
        playerPhysics.UPDATE_Physics();
        playerMovement.UPDATE_Movement();
    }

    // = Late Update =
    private void LateUpdate()
    {
        playerView.LATE_UPDATE_View();
    }

    // = Fixed Update =
    private void FixedUpdate()
    {
        playerMovement.FIXED_UPDATE_Movement();
        playerPhysics.FIXED_UPDATE_Physics();
    }
}