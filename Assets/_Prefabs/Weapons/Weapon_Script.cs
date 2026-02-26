using UnityEngine;

public abstract class WeaponScript : MonoBehaviour
{
    public Weapon_DataScript data;

    public Animator weaponAnimator;
    public Transform firePoint;

    protected int mask;

    protected virtual void Awake()
    {
        mask = ~LayerMask.GetMask("Player");
    }

    public abstract void PrimaryFire(Transform cameraPos);

    public abstract void SecondaryFire(Transform cameraPos);

    public virtual int CalculateCritDamage()
    {
        bool _;
        return CalculateCritDamage(out _);
    }

    public virtual int CalculateCritDamage(out bool isCrit)
    {
        float critRoll = Random.Range(0f, 100f);
        isCrit = critRoll <= data.critChance;
        if (isCrit)
        {
            return Mathf.RoundToInt(data.damage * data.critMultiplier);
        }
        return data.damage;
    }
}