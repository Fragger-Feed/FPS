using System;
using Unity.VisualScripting;
using UnityEngine;
public class PlayerController : MonoBehaviour
{
    PlayerInputScript playerInput;
    PlayerMovement playerMovement;
    [SerializeField] Camera cam;
    [SerializeField]  float sensitivity;
    private float x = 0;
    private float y = 0;
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
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        CameraController(playerInput.Player.Look.ReadValue<Vector2>());
    }

    private void FixedUpdate()
    {
        playerMovement.Movement(playerInput.Player.Move.ReadValue<Vector2>());
    }


    void CameraController(Vector2 input)
    {
        x -= input.y * sensitivity;
        y += input.x * sensitivity;
        x = Mathf.Clamp(x, -90, 90);
        cam.transform.localRotation = Quaternion.Euler(x, y, 0);
    }
}
