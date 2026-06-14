using UnityEngine;

public class HitscanBullet : MonoBehaviour
{
    public enum Owner
    { Player, Enemy };

    public float damage;
    public float distance;
    public float fadeTime;
    public float parryMultiplier;

    public Owner owner;
    public bool parried;
    public bool deflected;
    public Transform sourceTransform;

    [SerializeField] private LineRenderer lr;
    [SerializeField] private PlayerController player;

    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
        player = FindFirstObjectByType<PlayerController>();
    }

    private void Start()
    {
        Destroy(gameObject, 0.2f);

        Vector3 origin = transform.position;
        Vector3 end = transform.position + transform.forward * distance;

        RaycastHit hit;
        if (Physics.Raycast(origin, transform.forward, out hit, distance))
        {
            end = hit.point;
            HitSomething(hit);
        }

        lr.SetPosition(0, origin);
        lr.SetPosition(1, end);
    }

    private void HitSomething(RaycastHit hit)
    {
        if (owner == Owner.Player)
        {
            EntityStates enemy = hit.transform.GetComponent<EntityStates>();
            if (enemy == null) return;

            float damageFinal = parried ? damage * parryMultiplier : damage;
            enemy.Damage(false, Mathf.RoundToInt(damageFinal), null);
        }
        else
        {
            PlayerState playerState = hit.transform.GetComponent<PlayerState>();
            if (playerState == null) return;

            WeaponScript weapon = player.playerInventory.currentWeapon;
            if (weapon != null && weapon.IsInActiveWindow && weapon.data.weaponType == Weapon_DataScript.WeaponType.Melee)
            {
                SpawnDeflected(hit.point, Owner.Player, true);
                weapon.weaponAnimator.SetTrigger("Parry");
                weapon.didParry = true;
            }
            else if (weapon != null && weapon.IsInActiveWindow && weapon.data.weaponType == Weapon_DataScript.WeaponType.Ranged)
            {
                weapon.weaponAnimator.SetTrigger("Parry");
                weapon.didParry = true;
            }
            else
            {
                playerState.TakeDamage(Mathf.RoundToInt(damage));
            }
        }
    }

    private void SpawnDeflected(Vector3 origin, Owner newOwner, bool parried)
    {
        GameObject bulletObj = Instantiate(gameObject, origin, Quaternion.identity);
        HitscanBullet bullet = bulletObj.GetComponent<HitscanBullet>();

        bullet.owner = newOwner;
        bullet.deflected = true;
        bullet.damage = damage;
        bullet.parried = parried;

        if (newOwner == Owner.Player)
        {
            RaycastHit hit;
            Vector3 targetPoint;
            if (Physics.Raycast(player.CameraObj.transform.position, player.CameraObj.transform.forward, out hit, 200))
                targetPoint = hit.point;
            else
                targetPoint = player.CameraObj.transform.position + player.CameraObj.transform.forward;

            bulletObj.transform.LookAt(targetPoint);
        }
        else
            bulletObj.transform.LookAt(player.transform.position + Vector3.up);
    }
}