using UnityEngine;

public class BrokenDaggers_Controller : WeaponScript
{
    private float cooldown;

    public override void PrimaryFire(Transform cameraPos)
    {
        weaponAnimator.SetBool("Attack", true);
    }

    public override void SecondaryFire(Transform cameraPos)
    {
    }
}
