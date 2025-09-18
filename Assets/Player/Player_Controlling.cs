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
        playerView.UPDATE_View(ControllingData.Player.Look.ReadValue<Vector2>(), transform.position);
    }

    private void FixedUpdate()
    {
        playerMovement.UPDATE_Movement(ControllingData.Player.Move.ReadValue<Vector2>());
    }
}