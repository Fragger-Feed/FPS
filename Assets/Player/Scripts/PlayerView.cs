using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;
using UnityEngine.Windows;

public class PlayerView : MonoBehaviour
{
    // == Objects ==
    [SerializeField] private Camera CameraObj;

    // == Configs ==
    [SerializeField] private float sensitivity;

    [SerializeField] private Vector3 offset;
    [SerializeField] private Transform forward;

    // Hash
    private float x = 0;

    private float y = 0;

    // == Start ==
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // == UPDATE ==
    public void UPDATE_View(Vector3 InputData, Vector3 playerPos)
    {
        CameraObj.transform.position = playerPos + offset;

        x -= InputData.y * sensitivity;
        y += InputData.x * sensitivity;
        x = Mathf.Clamp(x, -90, 90);
        CameraObj.transform.localRotation = Quaternion.Euler(x, y, 0);
        forward.localRotation = Quaternion.Euler(0, y, 0);
    }
}