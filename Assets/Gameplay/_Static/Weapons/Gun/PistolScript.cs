using Unity.VisualScripting;
using UnityEngine;

public class PistolScript : WeaponScript
{
    float cooldown;
    public override void PrimaryFire(Transform cameraPos)
    {
        if (Time.time < cooldown)
            return;

        Debug.Log("Pew Pew suka");
        RaycastHit hit;
        if (Physics.Raycast(cameraPos.position, cameraPos.forward, out hit, data.range, mask))
        {
            Debug.Log("Hit " + hit.transform.name);
            Entity_Object entity = hit.transform.GetComponent<Entity_Object>();
            if (entity != null)
            {
                entity.Damage(data.damage);
            }
            Debug.DrawRay(cameraPos.position, cameraPos.forward * hit.distance, Color.red, 1f);
        }

        cooldown = Time.time + data.fireRate;
    }

    public override void SecondaryFire(Transform cameraPos)
    {
        Debug.Log("Secondary Pew Pew suka");
    }
}
