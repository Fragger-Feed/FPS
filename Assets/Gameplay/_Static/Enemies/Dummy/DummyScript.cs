using UnityEngine;

public class DummyScript : EnemieScript
{

    public override void Move()
    {
        nav.SetDestination(player.transform.position);
    }
}
