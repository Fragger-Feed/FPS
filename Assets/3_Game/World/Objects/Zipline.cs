using UnityEngine;

public class Zipline : InteractableOject
{
    private PlayerController player;

    private GameObject pointA;
    private GameObject pointB;
    private GameObject collider;

    private LineRenderer lineRenderer;
    private Transform targetEnd;
    [SerializeField] private Vector2 grabArea;
    private Vector3 direction;
    private float length;
    public bool isRiding = false;

    private void Awake()
    {
        pointA = transform.GetChild(0).gameObject;
        pointB = transform.GetChild(1).gameObject;
        collider = transform.GetChild(2).gameObject;

        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.SetPosition(0, pointA.transform.localPosition);
        lineRenderer.SetPosition(1, pointB.transform.localPosition);

        direction = (pointB.transform.position - pointA.transform.position);
        length = direction.magnitude;

        collider.transform.position = (pointA.transform.position + pointB.transform.position) * 0.5f;

        collider.transform.rotation = Quaternion.LookRotation(direction);
        collider.transform.localScale = new Vector3(grabArea.x, grabArea.y, length);
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

            float dotA = Vector3.Dot(player.ForwardObj.transform.forward, pointA.transform.forward);
            float dotB = Vector3.Dot(player.ForwardObj.transform.forward, pointB.transform.forward);

            targetEnd = dotB > dotA ? pointB.transform : pointA.transform;
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
        if (isRiding)
        {
            Vector3 toB = pointB.transform.position - player.transform.position;
            float projection = Vector3.Dot(toB, direction.normalized);
            projection = Mathf.Clamp(projection, 0, length);
            Vector3 targetPosition = pointB.transform.position - direction.normalized * projection;
            player.transform.position = Vector3.Lerp(player.transform.position, targetPosition, Time.deltaTime * 5f);
            player.transform.position = Vector3.Lerp(player.transform.position, targetEnd.position, Time.deltaTime * 5);
        }
    }
}