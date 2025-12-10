using UnityEngine;

public class PipeScript : WeaponScript
{
    Transform suka;
    public Vector3 offset;
    float cooldown;
    public override void PrimaryFire(Transform cameraPos)
    {
        if (Time.time < cooldown)
            return;

        Vector3 half = new Vector3(0.5f, 0.5f, data.range * 0.5f);
        Vector3 center = cameraPos.position + cameraPos.forward * (data.range * 0.5f);
        center += cameraPos.rotation * offset;

        Collider[] hit = Physics.OverlapBox(center, half, cameraPos.rotation);

        foreach (var item in hit)
        {
            Debug.Log("Hit " + item.transform.name);

            Entity_Object entity = item.GetComponent<Entity_Object>();
            if (entity != null)
                entity.Damage(data.damage);
        }

        suka = cameraPos;
        cooldown = Time.time + data.fireRate;
    }

    public override void SecondaryFire(Transform cameraPos)
    {
        Debug.Log("BONK BONK");
    }

    private void OnDrawGizmos()
    {
        if (suka == null) return;

        Gizmos.color = Color.red;

        Vector3 half = new Vector3(0.5f, 0.5f, data.range * 0.5f);
        Vector3 center = suka.position + suka.forward * (data.range * 0.5f);
        center += suka.rotation * offset;

        Gizmos.matrix = Matrix4x4.TRS(center, suka.rotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, half * 2f);
    }


}
