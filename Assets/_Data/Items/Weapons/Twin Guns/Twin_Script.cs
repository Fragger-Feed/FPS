using UnityEngine;

public class Twin_Script : WeaponScript
{
    [SerializeField] private GameObject bulletTrailLeft;
    [SerializeField] private LineRenderer bulletTrailRight;
    [SerializeField] private Transform shootPointL;
    [SerializeField] private Transform shootPointR;
    private bool canShoot = true;
    private bool isLeft;

    public override void PrimaryFire()
    {
        if (canShoot)
        {
            RaycastHit hit;
            Vector3 targetPoint;

            if (Physics.Raycast(cameraPos.position, cameraPos.forward, out hit, 200, mask))
                targetPoint = hit.point;
            else
                targetPoint = cameraPos.position + cameraPos.forward * 200;

            if (isLeft)
            {
                weaponAnimator.SetTrigger("Shoot L");
                GameObject bulletObj = Instantiate(bulletTrailLeft, shootPointL.position, Quaternion.identity);
                bulletObj.transform.LookAt(targetPoint);
                HitscanBullet bullet = bulletObj.GetComponent<HitscanBullet>();
                bullet.owner = HitscanBullet.Owner.Player;
                bullet.damage = CalculateCritDamage(data.rangedDamage);
            }
            else
            {
                weaponAnimator.SetTrigger("Shoot R");
                GameObject bulletObj = Instantiate(bulletTrailLeft, shootPointR.position, Quaternion.identity);
                bulletObj.transform.LookAt(targetPoint);
                HitscanBullet bullet = bulletObj.GetComponent<HitscanBullet>();
                bullet.owner = HitscanBullet.Owner.Player;
                bullet.damage = CalculateCritDamage(data.rangedDamage);
            }

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
            EntityStates enemy = item.GetComponentInParent<EntityStates>();
            if (enemy != null)
            {
                enemy.Damage(true, CalculateCritDamage(data.meleeDamage, out crit), this); if (crit) print("CRITICAL!!");
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