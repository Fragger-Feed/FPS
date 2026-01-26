using UnityEngine;

public class Zipline : InteractableOject
{
    PlayerController player;

    GameObject pointA;
    GameObject pointB;
    GameObject collider;

    private void Awake()
    {
        pointA = transform.GetChild(0).gameObject;
        pointB = transform.GetChild(1).gameObject;
        collider = transform.GetChild(2).gameObject;

    }


    public override void Interact(PlayerController _player)
    {
        player = _player;
    }
}
