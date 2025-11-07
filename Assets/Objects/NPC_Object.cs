using UnityEngine;

public class NPC_Object : MonoBehaviour
{
    [SerializeField] private GameObject dialogCameraHolder;
    [SerializeField] private PlayerController PlayerC;
    public bool inDialogWith;



    public void StartDialog() {
        inDialogWith = true;
        PlayerC.playerView.CameraHolder = dialogCameraHolder;
        PlayerC.playerView.toHolder = true;
        print("AAAAAAAAAAA");
    }

}
