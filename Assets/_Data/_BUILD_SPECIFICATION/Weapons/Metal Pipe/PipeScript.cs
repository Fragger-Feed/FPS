using UnityEngine;

public class PipeScript : WeaponScript
{
    private float cooldown;

    public override void PrimaryFire()
    {
        PerformMelee();
    }

    public override void PerformMelee()
    {
        base.PerformMelee();
    }

    public override void HitCheck()
    {
        if (didHit)
            return;

        Vector3 center = cameraPos.position + cameraPos.rotation * data.meleeHitPoint;
        Collider[] hit = Physics.OverlapBox(center, data.meleeHitArea / 2, cameraPos.rotation, mask);

        foreach (var item in hit)
        {
            Debug.Log("Hit " + item.transform.name);
            bool crit;
            EntityStates enemie = item.GetComponent<EntityStates>();
            if (enemie != null)
            {
                enemie.Damage(CalculateCritDamage(out crit)); if (crit) print("CRITICAL!!");
            }
        }
        didHit = true;
    }

    public override void SecondaryFire()
    {
        Debug.Log("BONK BONK");
    }
}