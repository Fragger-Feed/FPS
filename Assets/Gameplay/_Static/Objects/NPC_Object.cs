using UnityEngine;

public class NPC_Object : MonoBehaviour
{
    [SerializeField] private Controlling_Data Controlling;
    [SerializeField] private GameObject dialogCameraHolder;
    [SerializeField] private PlayerController PlayerC;
    public bool inDialogWith;



    public void StartDialog() {
        inDialogWith = true;
        PlayerC.playerView.CameraHolder = dialogCameraHolder;
        PlayerC.playerView.toHolder = true;
    }


    private void FixedUpdate()
    {
        if (PlayerC.ControllingData.Player.Jump.WasPressedThisFrame()) {
            PlayerC.playerView.CameraHolder = null;
            PlayerC.playerView.toHolder = false;
        }
    }

}
