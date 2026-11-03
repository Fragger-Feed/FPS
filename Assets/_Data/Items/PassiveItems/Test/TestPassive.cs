using UnityEngine;

public class TestPassive : PassiveItem_Script
{
    [SerializeField] private float speedAddition;

    public override void OnEquip(PlayerController player)
    {
        player.playerMovement.speed += speedAddition;
    }
}