using UnityEngine;

public class EnemieSpawner : ActiveItemScript
{
    [SerializeField] private GameObject prefab;

    public override void UseItem(PlayerController player)
    {
        RaycastHit hit;
        Physics.Raycast(player.transform.position, player.CameraObj.transform.forward, out hit);
        if (hit.collider != null)
            Instantiate(prefab, hit.point, new Quaternion(0, 0, 0, 0));
        print("Spawning Enemie");
    }
}