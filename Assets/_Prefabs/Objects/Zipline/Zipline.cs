using UnityEngine;

public class Zipline : InteractableOject
{
    private PlayerController player;

    private GameObject pointA;
    private GameObject pointB;
    private GameObject _collider;

    private Transform targetEnd;
    private Transform targetStart;

    private Vector3 ziplineVelocity;
    private Vector3 direction;
    private Vector3 directionNormalized;

    private float length;
    private float pos;

    public bool isRiding = false;

    [SerializeField] private float speed = 5f;
    [SerializeField] private float snapSpeed = 100;
    [SerializeField] private float jumpHeightLimit = 10;

    [SerializeField] private Vector2 grabArea;
    [SerializeField] private Vector3 offset;

    private void Awake()
    {
        pointA = transform.GetChild(0).gameObject;
        pointB = transform.GetChild(1).gameObject;
        _collider = transform.GetChild(2).gameObject;

        direction = (pointB.transform.position - pointA.transform.position);
        directionNormalized = direction.normalized;
        length = direction.magnitude;

        _collider.transform.position = (pointA.transform.position + pointB.transform.position) * 0.5f;

        _collider.transform.rotation = Quaternion.LookRotation(direction);
        _collider.transform.localScale = new Vector3(grabArea.x, grabArea.y, length);
    }

    public override void Interact(PlayerController _player)
    {
        if (_player == null) return;

        if (!isRiding)
        {
            AtachZipline(_player);
        }
        else
        {
            DeatachZipline(_player, true);
        }
    }

    private void Update()

    {
        if (!isRiding || player == null) return;

        if (player.playerMovement.isJumping || player.playerMovement.isDashing)
        {
            DeatachZipline(player, true);
            return;
        }
        float previousPos = pos;
        pos += Time.deltaTime * speed;

        Vector3 prevPosition = Vector3.Lerp(
            targetStart.position + offset,
            targetEnd.position + offset,
            previousPos
        );

        Vector3 newPosition = Vector3.Lerp(
            targetStart.position + offset,
            targetEnd.position + offset,
            pos
        );

        // calculate velocity
        ziplineVelocity = (newPosition - prevPosition) / Time.deltaTime;

        // move player
        player.transform.position = newPosition;

        if (pos > 1) { DeatachZipline(player, false); return; }

        /*        player.transform.position = Vector3.Lerp(
                    player.transform.position,
                    targetPosition,
                    Time.deltaTime * snapSpeed
                );

                player.transform.position = Vector3.Lerp(
                    player.transform.position,
                    targetEnd.position + offset,
                    Time.deltaTime * speed
                );*/
    }

    private float GetProjectionOnLine()
    {
        Vector3 ap = player.transform.position - targetStart.position;
        float ab2 = (targetEnd.position - targetStart.position).sqrMagnitude;
        float ap_ab = Vector3.Dot(ap, targetEnd.position - targetStart.position);
        return ap_ab / ab2;
    }

    private void AtachZipline(PlayerController _player)
    {
        player = _player;
        player.playerMovement.disableMovement = true;
        player.GetComponent<Collider>().enabled = false;
        player.RigidBodyC.interpolation = RigidbodyInterpolation.None;
        player.RigidBodyC.linearVelocity = Vector3.zero;
        player.playerMovement.isDashing = false;
        player.playerMovement.isJumping = false;
        player.playerPhysics.coyoteTime = 69;
        isRiding = true;
        pos = 0;
        float dotA = Vector3.Dot(player.ForwardObj.transform.forward, pointA.transform.forward);
        float dotB = Vector3.Dot(player.ForwardObj.transform.forward, pointB.transform.forward);

        targetEnd = dotB > dotA ? pointB.transform : pointA.transform;
        targetStart = dotB > dotA ? pointA.transform : pointB.transform;
        pos = GetProjectionOnLine();
    }

    private void DeatachZipline(PlayerController _player, bool manual)
    {
        player.GetComponent<Collider>().enabled = true;
        player.playerMovement.disableMovement = false;
        isRiding = false;
        float jumpHeight = (2 * player.playerMovement.jumpHeight / player.playerMovement.timeToApex);
        if (player.playerMovement.isJumping)
            player.RigidBodyC.linearVelocity = new Vector3(ziplineVelocity.x, Mathf.Clamp(ziplineVelocity.y < 0 ? jumpHeight : jumpHeight + ziplineVelocity.y, 0, jumpHeight + jumpHeightLimit), ziplineVelocity.z);
        else if (manual)
            player.RigidBodyC.linearVelocity = ziplineVelocity;
        else
            player.RigidBodyC.linearVelocity = Vector2.zero;
        player.RigidBodyC.interpolation = RigidbodyInterpolation.Interpolate;
        print(player.RigidBodyC.linearVelocity);
        player = null;
    }
}