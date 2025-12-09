using UnityEngine;

public class PipeScript : WeaponScript
{
    public override void PrimaryFire()
    {
        Debug.Log("BONK");
    }
    public override void SecondaryFire()
    {
        Debug.Log("BONK BONK");
    }
}
