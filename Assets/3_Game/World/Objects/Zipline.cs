using UnityEngine;

public class Zipline : InteractableOject
{
    private PlayerController player;

    private GameObject pointA;
    private GameObject pointB;
    private GameObject _collider;

    private LineRenderer lineRenderer;
    private Transform targetEnd;
    private Transform targetStart;
    [SerializeField] private Vector2 grabArea;
    [SerializeField] private Vector3 offset;
    private Vector3 direction;
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

        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.SetPosition(0, pointA.transform.localPosition);
        lineRenderer.SetPosition(1, pointB.transform.localPosition);

        direction = (pointB.transform.position - pointA.transform.position);
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
        }
        else
        {
            player.GetComponent<Collider>().enabled = true;
            player.disableMovement = false;
            isRiding = false;
            player = null;
        }
    }

    private void Update()

    {
        if (!isRiding || player == null) return;

        Vector3 dir = direction.normalized;

        Vector3 toPlayer = player.transform.position - pointA.transform.position;
        float projection = Vector3.Dot(toPlayer, dir);
        projection = Mathf.Clamp(projection, 0f, length);

        Vector3 closestPoint = pointA.transform.position + dir * projection;

        Vector3 right = Vector3.Cross(Vector3.up, dir).normalized;
        Vector3 downFromCable = Vector3.Cross(dir, right).normalized;

        Vector3 targetPosition =
            closestPoint +
            right * offset.x +
            downFromCable * offset.y +
            dir * offset.z;

        pos += Time.deltaTime * speed;
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
}