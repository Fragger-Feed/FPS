using UnityEngine;

public class DummyScript : EnemieScript
{

    public override void Move()
    {
        base.Move();

        nav.SetDestination(player.transform.position);
    }

    public override void Attack()
    {
        Debug.Log("Dummy Attacking!");
    }
}
