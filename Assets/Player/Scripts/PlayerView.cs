using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;
using UnityEngine.Windows;

public class PlayerView : MonoBehaviour
{
    // == Objects ==
    [SerializeField] private Camera CameraObj;
    [SerializeField] private Transform forward;

    // == Configs ==
    [SerializeField] private float sensitivity;
    [SerializeField] private Vector3 offset;


    // Hash
    public Vector2 Hash = Vector2.zero;



    // == START ==
    private void Start() {
        Cursor.lockState = CursorLockMode.Locked;
    }


    // == UPDATE ==
    public void UPDATE_View(Vector3 InputData, Vector3 playerPos) {

        CameraObj.transform.position = playerPos + offset;

        Hash.x = Mathf.Clamp(Hash.x - InputData.y * sensitivity, -90, 90);
        Hash.y += InputData.x * sensitivity;

        CameraObj.transform.localRotation = Quaternion.Euler(Hash.x, Hash.y, 0);
        forward.localRotation = Quaternion.Euler(0, Hash.y, 0);
    }
}