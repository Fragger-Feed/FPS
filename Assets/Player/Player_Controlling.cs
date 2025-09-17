using System;
using Unity.VisualScripting;
using UnityEngine;
public class PlayerController : MonoBehaviour
{
    // == Player Scripts ==
    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] PlayerView playerView;



    // == Data ==
    [SerializeField] Controlling_Data ControllingData;






    private void OnEnable()
    {
        ControllingData.Enable();
    }



    private void Update()
    {
        playerView.UPDATE_View(ControllingData.Player.Look.ReadValue<Vector2>());
    }

    private void FixedUpdate()
    {
        playerMovement.UPDATE_Movement(ControllingData.Player.Move.ReadValue<Vector2>());
    }


   
}
