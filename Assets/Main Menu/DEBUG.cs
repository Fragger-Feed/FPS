using UnityEngine;

public class DEBUG : MonoBehaviour
{

    Controlling_Data data;
    private void Awake() { data = new Controlling_Data(); }
    private void OnEnable() { data.Enable(); }
    void Update()
    {
        if (data.Player.Debug.WasPressedThisFrame())
        {
            print("ZALUPA");
        }
    }
}
