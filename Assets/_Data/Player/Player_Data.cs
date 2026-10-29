using UnityEngine;

[CreateAssetMenu(fileName = "Player_Data", menuName = "Player_Data")]
public class Player_Data : ScriptableObject
{
    [Header("Movement")]
    public float speed;

    public float acceleration;
    public float timeToApex;
    public float jumpHeight;
    public float dashTime;
    public float dashSpeed;
}