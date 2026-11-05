using UnityEngine;

public class LootRoomScript : InteractableOject
{
    [SerializeField] private Weapon_DataScript[] weaponLootPool;
    [SerializeField] private ActiveItem_DataScript[] activeItemLootPool;
    [SerializeField] private PassiveItem_DataScript passiveItemLootPool;

    public override void Interact(PlayerController player)
    {
    }
}