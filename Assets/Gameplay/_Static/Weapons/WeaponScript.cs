using UnityEngine;

public abstract class WeaponScript : MonoBehaviour
{
    public Weapons_Data data;

    public Animator weaponAnimator;
    public Transform firePoint;

    public abstract void PrimaryFire(Transform cameraPos);
    public abstract void SecondaryFire(Transform cameraPos);
}
