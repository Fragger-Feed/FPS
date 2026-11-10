using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;

public class LootRoomScript : InteractableOject
{
    [SerializeField] private Weapon_DataScript[] weaponLootPool;
    [SerializeField] private ActiveItem_DataScript[] activeItemLootPool;
    [SerializeField] private PassiveItem_DataScript[] passiveItemLootPool;

    private Item_DataScript[][] pools;

    private void Awake()
    {
        pools = new Item_DataScript[][] { weaponLootPool, activeItemLootPool, passiveItemLootPool };
    }

    public override void Interact(PlayerController player)
    {
        for (int i = 0; i < 3; i++)
        {
            Item_DataScript item = RollItem();
        }

        /*        if (item is Weapon_DataScript weapon)
                {
                    player.playerInventory.InventoryData.WeaponsList.Add(weapon);
                }
                else if (item is ActiveItem_DataScript active)
                {
                    player.playerInventory.InventoryData.ActiveItemsList.Add(active);
                }
                else if (item is PassiveItem_DataScript passive)
                {
                    player.playerInventory.InventoryData.PassiveItemList.Add(passive);
                }*/
    }

    private Item_DataScript RollItem()
    {
        Item_DataScript[] itemPool = pools[Random.Range(0, pools.Length)];

        if (itemPool.Length == 0) return null;

        return itemPool[Random.Range(0, itemPool.Length)];
    }
}