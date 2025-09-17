using UnityEngine;
using UnityEngine.Windows;

public class PlayerView : MonoBehaviour
{

    // == Objects ==
    [SerializeField] Camera CameraObj;


    // == Configs ==
    [SerializeField] float sensitivity;


    // Hash
    private float x = 0;
    private float y = 0;



    // == Start ==
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }



    // == UPDATE ==
    public void UPDATE_View(Vector3 InputData) 
    {
        x -= InputData.y * sensitivity;
        y += InputData.x * sensitivity;
        x = Mathf.Clamp(x, -90, 90);
        CameraObj.transform.localRotation = Quaternion.Euler(x, y, 0);
    }
}
