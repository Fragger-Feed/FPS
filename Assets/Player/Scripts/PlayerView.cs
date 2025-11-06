using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;
using UnityEngine.Windows;

public class PlayerView : MonoBehaviour
{
    // == Data ==
    [Header("Data")]
    [SerializeField] private PlayerController PlayerC;

    // == Configs ==
    [Header("Configs")]
    [SerializeField] private float sensitivity;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float viewDistance;


    // Hash
    [Header("Hash")]
    public List<InteractionObject> InteractionObjects = new List<InteractionObject>();
    public Vector2 Hash = Vector2.zero;



    // == START ==
    private void Start() {
        Cursor.lockState = CursorLockMode.Locked;
    }


    // == UPDATE ==
    public void UPDATE_View() {
        UpdateInteractions();
    }

    public void late_Update_View()
    {
        CameraRotate();
    }



    // Camera Rotate
    public void CameraRotate() {
        Vector3 InputData = PlayerC.ControllingData.Player.Look.ReadValue<Vector2>();

        PlayerC.CameraObj.transform.position = transform.position + offset;

        Hash.x = Mathf.Clamp(Hash.x - InputData.y * sensitivity, -90, 90);
        Hash.y += InputData.x * sensitivity;

        PlayerC.CameraObj.transform.localRotation = Quaternion.Euler(Hash.x, Hash.y, 0);
        PlayerC.ForwardObj.transform.localRotation = Quaternion.Euler(0, Hash.y, 0);
    }


    // Interactions
    public void UpdateInteractions() {

        // ON
        List<InteractionObject> CurrentInteractionObjects = new List<InteractionObject>();
        foreach (RaycastHit raycastHit in Physics.RaycastAll(PlayerC.CameraObj.transform.position, PlayerC.CameraObj.transform.forward, viewDistance).Reverse()) {
            if (raycastHit.transform.TryGetComponent(out InteractionObject Interaction)) {
                CurrentInteractionObjects.Add(Interaction);
                if (!InteractionObjects.Contains(Interaction) && Interaction.interactionAbility) {
                    InteractionObjects.Add(Interaction);
                    Interaction.Interaction_ON_Function();
                }
            }
            break;
        }

        // OFF | START
        foreach(InteractionObject Interaction in InteractionObjects.Except(CurrentInteractionObjects).ToArray()) {
            InteractionObjects.Remove(Interaction);
            Interaction.Interaction_OFF_Function();
        }

        // START
        if (PlayerC.ControllingData.Player.Interact.WasPressedThisFrame()) {
            foreach (InteractionObject Interaction in InteractionObjects.ToArray()) {
                InteractionObjects.Remove(Interaction);
                Interaction.Interaction_START_Function();
            }
        }

    }
}