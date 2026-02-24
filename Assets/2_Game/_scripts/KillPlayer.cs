using UnityEngine;

public class KillPlayer : InteractableOject
{
    public override void Interact(PlayerController player)
    {
        player.playerState.TakeDamage(25);
    }
}
