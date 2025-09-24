using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Scriptable Objects/PlayerData")]
public class PlayerData : ScriptableObject
{
    [SerializeField] public float speed;

    [SerializeField] public float acceleration;

    [SerializeField] public float timeToApex;

    [SerializeField] public float jumpHeight;

    [SerializeField] public int heath;
}