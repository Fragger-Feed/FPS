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
        bulletTrailLeft.widthCurve
    }

    public override void SecondaryFire()
    { }

    public override void HitCheck()
    {
    }

    public void MakeReadyToShoot()
    {
        canShoot = true;
    }
}