using System;
using System.Data.Common;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // == Data ==
    [Header("Data")]
    [SerializeField] public Controlling_Data ControllingData;

    // == Player Scripts ==
    [Header("Player Scripts")]
    [SerializeField] public PlayerMovement playerMovement;
    [SerializeField] public PlayerView playerView;
    [SerializeField] public PlayerPhysics playerPhysics;

    // == Components ==
    [Header("Components")]
    public Rigidbody RigidBodyC;
    public CapsuleCollider playerCollider;


    // == Objects ==
    [Header("Objects")]
    public GameObject ForwardObj;
    public Camera CameraObj;



    // == Start ==
    private void Awake() { ControllingData = new Controlling_Data(); }
    private void OnEnable() { ControllingData.Enable(); }



    // == Update ==
    private void Update() {
        playerView.UPDATE_View();
        playerPhysics.UPDATE_Physics();
        playerMovement.UPDATE_Movement();
    }

    private void FixedUpdate() {
        playerMovement.FIXED_UPDATE_Movement();
        playerPhysics.FIXED_UPDATE_Physics();
    }

    
}