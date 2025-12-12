using UnityEngine;

public abstract class WeaponScript : MonoBehaviour
{
    public Weapons_Data data;

    public Animator weaponAnimator;
    public Transform firePoint;

    protected int mask;

    protected virtual void Awake()
    {
        mask = ~LayerMask.GetMask("Player");
        weaponAnimator = GetComponent<Animator>();
    }

    public abstract void PrimaryFire(Transform cameraPos);
    public abstract void SecondaryFire(Transform cameraPos);
}
