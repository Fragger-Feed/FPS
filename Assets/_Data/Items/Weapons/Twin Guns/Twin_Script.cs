using UnityEngine;

public class Twin_Script : WeaponScript
{
    [SerializeField] private LineRenderer bulletTrailLeft;
    [SerializeField] private LineRenderer bulletTrailRight;
    private bool canShoot = true;
    private bool isLeft;

    public override void PrimaryFire()
    {
        if (canShoot)
        {
            if (isLeft)
                weaponAnimator.SetTrigger("Shoot L");
            else
                weaponAnimator.SetTrigger("Shoot R");

            isLeft = !isLeft;
            canShoot = false;
        }
    }

    private void Update()
    {
        /*        bulletTrailLeft.widthCurve*/
    }

    public override void SecondaryFire()
    { }

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
            EntityStates enemy = item.GetComponent<EntityStates>();
            if (enemy != null)
            {
                enemy.Damage(true, CalculateCritDamage(out crit), this); if (crit) print("CRITICAL!!");
                enemy.enemyScript.lastPlayerAttackTime = lastHitTime;
            }
        }
        didHit = true;
    }

    public void MakeReadyToShoot()
    {
        canShoot = true;
    }
}