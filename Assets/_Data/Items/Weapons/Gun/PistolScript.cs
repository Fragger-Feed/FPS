using Unity.VisualScripting;
using UnityEngine;

public class PistolScript : WeaponScript
{
    private float cooldown;

    public override void PrimaryFire()
    {
        if (Time.time < cooldown)
            return;

        Debug.Log("Pew Pew suka");
        RaycastHit hit;
        if (Physics.Raycast(cameraPos.position, cameraPos.forward, out hit, data.range, mask))
        {
            Debug.Log("Hit " + hit.transform.name);
            EntityStates enemie = hit.transform.GetComponent<EntityStates>();
            bool crit;
            if (enemie != null)
            {
                enemie.Damage(false, CalculateCritDamage(out crit));
                if (crit)
                    print("CRITICAL!!!");
            }

            Debug.DrawRay(cameraPos.position, cameraPos.forward * hit.distance, Color.red, 1f);
        }

        cooldown = Time.time + data.fireRate;
    }

    public override void SecondaryFire()
    {
        RaycastHit hit;
        if (Physics.Raycast(cameraPos.position, cameraPos.forward, out hit, data.range))
            print(hit.normal);
    }

    public override void HitCheck()
    {
    }
}