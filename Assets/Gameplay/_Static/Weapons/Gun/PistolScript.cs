using UnityEngine;

public class PistolScript : WeaponScript
{
    public override void PrimaryFire()
    {
        Debug.Log("Pew Pew suka");
    }

    public override void SecondaryFire()
    {
        Debug.Log("Secondary Pew Pew suka");
    }
}
