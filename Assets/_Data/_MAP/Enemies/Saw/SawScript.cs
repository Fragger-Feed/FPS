using UnityEngine;

public class SawScript : MonoBehaviour
{
    public GameObject Player;
    public Rigidbody Rigidbody;

    public float Acceleration;

    private void Awake()
    {
        Player = GameObject.Find("PlayerObj");
    }

    void FixedUpdate()
    {
        transform.LookAt(Player.transform);
        Rigidbody.AddForce(transform.forward * Acceleration);
    }
}
