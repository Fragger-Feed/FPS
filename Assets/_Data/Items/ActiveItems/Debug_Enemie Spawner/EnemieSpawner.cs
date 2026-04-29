using UnityEngine;

public class EnemieSpawner : ActiveItem_Script
{
    [SerializeField] private GameObject prefab;

    public override void UseActiveItem(PlayerController player)
    {
        RaycastHit hit;
        Physics.Raycast(player.transform.position, player.CameraObj.transform.forward, out hit);
        if (hit.collider != null)
            Instantiate(prefab, hit.point, new Quaternion(0, 0, 0, 0));
        print("Spawning Enemie");
    }
}