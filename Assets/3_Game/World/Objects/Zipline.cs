using UnityEngine;

public class Zipline : InteractableOject
{
    private PlayerController player;

    private GameObject pointA;
    private GameObject pointB;
    private GameObject _collider;

    private Transform targetEnd;
    private Transform targetStart;
    [SerializeField] private Vector2 grabArea;
    [SerializeField] private Vector3 offset;
    private Vector3 direction;
    private Vector3 directionNormalized;
    private float length;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float snapSpeed = 100;
    public bool isRiding = false;
    private float pos;

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
            DeatachZipline(_player);
        }
    }

    private void Update()

    {
        if (!isRiding || player == null) return;

        /*
                Vector3 right = Vector3.Cross(Vector3.up, directionNormalized).normalized;
                Vector3 downFromCable = Vector3.Cross(directionNormalized, right).normalized;

                Vector3 targetPosition =
                    closestPoint +
                    right * offset.x +
                    downFromCable * offset.y +
                    directionNormalized * offset.z;*/

        pos += Time.deltaTime * speed;
        if (pos > 1) DeatachZipline(player);
        player.transform.position = Vector3.Lerp(targetStart.position + offset, targetEnd.position + offset, pos);

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
        player.disableMovement = true;
        player.GetComponent<Collider>().enabled = false;
        player.RigidBodyC.linearVelocity = Vector3.zero;
        isRiding = true;
        pos = 0;
        float dotA = Vector3.Dot(player.ForwardObj.transform.forward, pointA.transform.forward);
        float dotB = Vector3.Dot(player.ForwardObj.transform.forward, pointB.transform.forward);

        targetEnd = dotB > dotA ? pointB.transform : pointA.transform;
        targetStart = dotB > dotA ? pointA.transform : pointB.transform;
        pos = GetProjectionOnLine();
    }

    private void DeatachZipline(PlayerController _player)
    {
        player.GetComponent<Collider>().enabled = true;
        player.disableMovement = false;
        isRiding = false;
        player = null;
    }
}