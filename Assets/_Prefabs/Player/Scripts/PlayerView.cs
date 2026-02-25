using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;
using UnityEngine.Windows;

public class PlayerView : MonoBehaviour
{
    // == Data ==
    [Header("Data")]
    [SerializeField] public PlayerController PlayerC;

    // == Configs ==
    [Header("Configs")]
    public float sensitivity;
    public Vector3 offset;
    public float viewDistance;
    public bool toHolder;

    // == Objects ==
    [Header("Objects")]
    public GameObject CameraHolder;

    // Hash
    [Header("Hash")]
    public InteractionObject interactionObject = null;
    public float InteractionHolding_StartTime;
    public bool InteractionHolding;
    public Vector2 Hash = Vector2.zero;



    // === START ===
    private void Start() {
        Cursor.lockState = CursorLockMode.Locked;
    }



    // === UPDATE ===
    // = Update =
    public void UPDATE_View() {
        if (!toHolder) { UpdateInteractions(); }
    }
    // = Late Update =
    public void LATE_UPDATE_View() {
        if (!toHolder) { CameraRotate(); }
    }



    // == Camera Rotation ==
    public void CameraRotate() {
        Vector3 InputData = PlayerC.ControllingData.Player.Look.ReadValue<Vector2>();

        PlayerC.CameraObj.transform.position = transform.position + offset;

        Hash.x = Mathf.Clamp(Hash.x - InputData.y * sensitivity, -90, 90);
        Hash.y += InputData.x * sensitivity;

        PlayerC.CameraObj.transform.localRotation = Quaternion.Euler(Hash.x, Hash.y, 0);
        PlayerC.ForwardObj.transform.localRotation = Quaternion.Euler(0, Hash.y, 0);
    }



    // == Interactions ==
    public void UpdateInteractions() {
        // == View ==
        // on | off
        RaycastHit[] Objs = Physics.RaycastAll(PlayerC.CameraObj.transform.position, PlayerC.CameraObj.transform.forward, viewDistance);
        if (Objs.Length != 0) {
            RaycastHit HitObject = Physics.RaycastAll(PlayerC.CameraObj.transform.position, PlayerC.CameraObj.transform.forward, viewDistance)[0];
            if (HitObject.transform.TryGetComponent(out InteractionObject Interaction)) {
                if (Interaction != interactionObject) {
                    if (interactionObject != null) { interactionObject.Interaction_LOOK_OFF(); }
                    interactionObject = Interaction;
                    interactionObject.Interaction_LOOK_ON();
                }
            }
        }
        else if (interactionObject != null) { interactionObject.Interaction_LOOK_OFF(); interactionObject = null; }


        // == Interaction ==
        // start
        if (PlayerC.ControllingData.Player.Interact.WasPressedThisFrame()) {
            interactionObject.Interaction_START();
        }
        // end
        if (PlayerC.ControllingData.Player.Interact.WasReleasedThisFrame()) {
            interactionObject.Interaction_END();
        }
        // Holding
        if (PlayerC.ControllingData.Player.Interact.IsPressed()) {
            if (InteractionHolding) { interactionObject.Interaction_HOLDING_TIME(Time.time - InteractionHolding_StartTime); interactionObject.Interaction_HOLDING(); }
            else { InteractionHolding = true; InteractionHolding_StartTime = Time.time; }
        }
    }



    // == Connect To Holder ==
    public void ConnectToHolder() {
        PlayerC.CameraObj.transform.position = CameraHolder.transform.position;
    }
}