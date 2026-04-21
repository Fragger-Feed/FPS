using UnityEngine;

public class BrokenDaggers_Controller : WeaponScript
{
    private float cooldown;

    public override void PrimaryFire()
    {
        weaponAnimator.SetBool("Attack", true);
    }

    public override void SecondaryFire()
    {
    }

    public override void HitCheck()
    {
    }
}