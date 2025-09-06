using System;
using UnityEngine;
public class PlayerController : MonoBehaviour
{
    PlayerInputScript playerInput;
    PlayerMovement playerMovement;

    private void Awake()
    {
        playerInput = new PlayerInputScript();
    }

    private void OnEnable()
    {
        playerInput.Enable();
    }

    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void FixedUpdate()
    {
        playerMovement.Movement(playerInput.Player.Move.ReadValue<Vector2>());
    }
}
