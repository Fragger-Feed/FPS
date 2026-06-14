using UnityEngine;

public class Shooter_CopScript : Enemy_Script
{
    [SerializeField] private float tooCloseRange;
    [SerializeField] private float strafeSpeedMin;
    [SerializeField] private float strafeSpeedMax;
    [SerializeField] private float strafeSpeed;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private float lockOnTime;
    [SerializeField] private float flashTime;
    [SerializeField] private float lockOnAngle;

    [SerializeField] private Vector3 strafeDirection;

    [SerializeField] private Transform shootPoint;

    [SerializeField] private LineRenderer laserSight;

    [SerializeField] private Material[] sightColors = new Material[2];

    [SerializeField] private GameObject hitscanBulletPrefab;

    private float strafeTime;
    private float lockOnTimer;

    public override void Move()
    {
        base.Move();

        float distance = Vector3.Distance(player.transform.position, transform.position);

        animator.SetTrigger("Move");
        animator.ResetTrigger("Attack");

        if (distance < tooCloseRange)
        {
            strafeTime -= Time.deltaTime;

            Vector3 awayDirection = (transform.position - player.transform.position).normalized;

            if (strafeTime < 0)
            {
                strafeDirection = Random.Range(0, 2) >= 0.5f ? transform.right * strafeSpeed : -transform.right * strafeSpeed;
                strafeTime = Random.Range(1, 4);
                strafeSpeed = Random.Range(strafeSpeedMin, strafeSpeedMax);
            }

            Vector3 finalDirection = awayDirection * 2 + strafeDirection;

            nav.SetDestination(transform.position + finalDirection);
        }
        else if (distance < data.attackRange)
        {
            nav.SetDestination(transform.position);

            currentState = EnemyState.Attack;
        }
        else
        {
            nav.SetDestination(player.transform.position);
        }
    }

    public override void Attack()
    {
        base.Attack();

        float distance = Vector3.Distance(player.transform.position, transform.position);
        if (distance < tooCloseRange || distance > data.attackRange)
            currentState = EnemyState.Move;

        FacePlayer();

        if (Time.time < cooldownTimer) return;

        Vector3 playerDirection = (player.transform.position - transform.position).normalized;
        float angle = Vector3.Angle(transform.forward, playerDirection);

        if (lockOnTimer > flashTime)
        {
            laserSight.material = sightColors[1];

            WeaponScript weapon = player.playerInventory.currentWeapon;
            if (weapon != null && weapon.IsInActiveWindow)
            {
                Shoot();
            }
        }
        else
            laserSight.material = sightColors[0];

        if (angle < lockOnAngle)
        {
            lockOnTimer += Time.deltaTime;

            if (lockOnTimer > lockOnTime)
            {
                Shoot();
            }
        }
        else
        {
            lockOnTimer = 0;
        }
    }

    public void Damage()
    {
        currentState = EnemyState.Move;
    }

    private void FacePlayer()
    {
        Vector3 direction = player.transform.position - transform.position;
        direction.y = 0;
        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), Time.fixedDeltaTime * rotateSpeed);
        }
    }

    private void Shoot()
    {
        GameObject bulletObj = Instantiate(hitscanBulletPrefab, shootPoint.position, shootPoint.rotation);
        HitscanBullet bullet = bulletObj.GetComponent<HitscanBullet>();
        bullet.owner = HitscanBullet.Owner.Enemy;
        bullet.damage = data.attackDamage;

        laserSight.material = sightColors[0];
        lockOnTimer = 0;
        cooldownTimer = Time.time + data.attackCooldown;
    }

    public override void Update()
    {
        base.Update();

        laserSight.SetPosition(0, shootPoint.position);

        RaycastHit hit;
        if (Physics.Raycast(shootPoint.position, shootPoint.forward, out hit, 100))
        {
            laserSight.SetPosition(1, hit.point);
        }
        else
        {
            laserSight.SetPosition(1, shootPoint.position + shootPoint.forward * 100);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, data.attackRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, tooCloseRange);
    }
}