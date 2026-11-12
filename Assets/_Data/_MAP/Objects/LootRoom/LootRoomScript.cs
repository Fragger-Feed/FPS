using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LootRoomScript : InteractableOject
{
    [SerializeField] private Weapon_DataScript[] weaponLootPool;
    [SerializeField] private ActiveItem_DataScript[] activeItemLootPool;
    [SerializeField] private PassiveItem_DataScript[] passiveItemLootPool;

    private Item_DataScript[][] pools;

    [SerializeField] private GameObject canvas;

    [Header("Choose")]
    [SerializeField] private GameObject choosePanel;

    [SerializeField] private GameObject[] chooseItemSlots;

    [Header("Replace")]
    [SerializeField] private GameObject replacePanel;

    [SerializeField] private GameObject[] equipedWeaponsSlots;
    [SerializeField] private GameObject[] equipedActiveItemSlots;
    /*    [SerializeField] GameObject */

    private List<Item_DataScript> items;

    private PlayerController player;

    private void Awake()
    {
        pools = new Item_DataScript[][] { weaponLootPool, activeItemLootPool, passiveItemLootPool };
    }

    public override void Interact(PlayerController _player)
    {
        player = _player;
        canvas.SetActive(true);
        choosePanel.SetActive(true);

        Cursor.lockState = CursorLockMode.None;

        items = new List<Item_DataScript>();

        for (int i = 0; i < 3; i++)
        {
            Item_DataScript item = RollItem();
            items.Add(item);
            if (item == null) continue;

            if (item.icon != null)
                chooseItemSlots[i].transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = item.icon;
            if (item.itemName != "")
                chooseItemSlots[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = item.itemName;
        }
    }

    public void OnItemSelection(GameObject slot)
    {
        Item_DataScript item = items[Array.IndexOf(chooseItemSlots, slot)];
        PlayerInventory inventory = player.playerInventory;
        Inventory_Data inventoryData = inventory.InventoryData;
        if (item is Weapon_DataScript weapon)
        {
            for (int i = 0; i < inventoryData.WeaponsList.Count; i++)
            {
                if (inventoryData.WeaponsList[i] == null)
                {
                    inventoryData.WeaponsList[i] = weapon;
                    break;
                }
            }
        }
        else if (item is ActiveItem_DataScript active)
        {
            for (int i = 0; i < inventoryData.ActiveItemsList.Count; i++)
            {
                if (inventoryData.ActiveItemsList[i] == null)
                {
                    inventoryData.ActiveItemsList[i] = active;
                    break;
                }
            }
        }
        else if (item is PassiveItem_DataScript passive)
        {
            player.playerInventory.InventoryData.PassiveItemList.Add(passive);
        }

        canvas.SetActive(false);
        choosePanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;

        inventory.UpdateSlots();
        inventory.UpdateWeapon();
        inventory.UpdateActiveItemSlots();
        inventory.EnableAllPassives();
    }

    private Item_DataScript RollItem()
    {
        Item_DataScript[] itemPool = pools[UnityEngine.Random.Range(0, pools.Length)];

        if (itemPool.Length == 0) return null;

        return itemPool[UnityEngine.Random.Range(0, itemPool.Length)];
    }
}