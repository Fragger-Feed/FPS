using UnityEngine;

public abstract class WeaponScript : MonoBehaviour
{
    public Weapon_DataScript data;

    public Animator weaponAnimator;
    public Transform firePoint;
    public Transform cameraPos;

    public bool isHitting;
    public bool canHit;
    public bool didHit;
    public bool didParry;
    public bool IsInActiveWindow => Time.time < hittingTimer;

    public float hittingTimer;
    public float lastHitTime;

    protected int mask;

    private PlayerController player;

    protected virtual void Awake()
    {
        mask = ~LayerMask.GetMask("Player");
        player = FindFirstObjectByType<PlayerController>();
        cameraPos = player.CameraObj.transform;
    }

    public virtual void FixedUpdate()
    {
        if (Time.time < hittingTimer)
        {
            HitCheck();
        }
    }

    public abstract void PrimaryFire();

    public abstract void SecondaryFire();

    public virtual void PerformMelee()
    {
        canHit = false;
        didHit = false;
        didParry = false;
        lastHitTime = Time.time;

        weaponAnimator.SetTrigger("Hit");
    }

    public virtual void StartHitting()
    {
        hittingTimer = Time.time + data.hitTime;
        lastHitTime = Time.time;
    }

    public virtual void StopHitting()
    {
    }

    public abstract void HitCheck();

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

    private void OnCollisionEnter(Collision collision)
    {
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(new Vector3(1, 0, 0), Vector3.up);
        Gizmos.color = (Time.time < hittingTimer) ? Color.red : Color.yellow;
        Vector3 center = cameraPos.position + cameraPos.rotation * data.meleeHitPoint;
        Gizmos.matrix = Matrix4x4.TRS(center, cameraPos.rotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, data.meleeHitArea);
        Gizmos.color = Color.pink;
        Vector3 centerParry = cameraPos.position + cameraPos.rotation * data.projectileParryPoint;
        Gizmos.matrix = Matrix4x4.TRS(centerParry, cameraPos.rotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, data.projectileParryArea);
    }
}