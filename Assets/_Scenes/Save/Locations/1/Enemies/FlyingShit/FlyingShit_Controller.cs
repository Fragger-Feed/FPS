using UnityEngine;

public class FlyingShit_Controller : MonoBehaviour
{
    public GameObject Shield;
    public GameObject Shield2;

    private void FixedUpdate()
    {
        Shield.transform.Rotate(new Vector3(1, 0, 1));
        Shield2.transform.Rotate(new Vector3(-1, 0, 1));
    }
}
