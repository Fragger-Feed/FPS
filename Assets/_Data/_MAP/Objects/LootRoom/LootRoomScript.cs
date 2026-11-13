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

    [SerializeField] private GameObject[] itemCategory;
    [SerializeField] private GameObject[] equipedWeaponsSlots;
    [SerializeField] private GameObject[] equipedActiveItemSlots;
    [SerializeField] private GameObject replacingObject;

    private Item_DataScript replacingItem;
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
        replacePanel.SetActive(false);

        player.ControllingData.Player.Get().SetActive(false);
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
                    UpdateAndDissableAll();
                    break;
                }
            }
            Replace(weapon);
        }
        else if (item is ActiveItem_DataScript active)
        {
            for (int i = 0; i < inventoryData.ActiveItemsList.Count; i++)
            {
                if (inventoryData.ActiveItemsList[i] == null)
                {
                    inventoryData.ActiveItemsList[i] = active;
                    UpdateAndDissableAll();
                    break;
                }
            }
            Replace(active);
        }
        else if (item is PassiveItem_DataScript passive)
        {
            player.playerInventory.InventoryData.PassiveItemList.Add(passive);
            UpdateAndDissableAll();
        }
    }

    private void Replace(Item_DataScript item)
    {
        replacePanel.SetActive(true);
        choosePanel.SetActive(false);
        replacingItem = item;

        if (item is Weapon_DataScript weapon)
        {
            itemCategory[0].SetActive(true);
            itemCategory[1].SetActive(false);

            for (int i = 0; i < equipedWeaponsSlots.Length; i++)
            {
                if (player.playerInventory.InventoryData.WeaponsList[i].icon != null)
                    equipedWeaponsSlots[i].transform.GetChild(0).GetComponent<Image>().sprite = player.playerInventory.InventoryData.WeaponsList[i].icon;
            }
        }
        else if (item is ActiveItem_DataScript active)
        {
            itemCategory[0].SetActive(false);
            itemCategory[1].SetActive(true);
            for (int i = 0; i < equipedActiveItemSlots.Length; i++)
            {
                if (player.playerInventory.InventoryData.ActiveItemsList[i].icon != null)
                    equipedActiveItemSlots[i].transform.GetChild(0).GetComponent<Image>().sprite = player.playerInventory.InventoryData.ActiveItemsList[i].icon;
            }
        }
        if (item.icon != null)
            replacingObject.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = item.icon;
    }

    public void OnHover(GameObject obj)
    {
        replacingObject.transform.position = new Vector3(replacingObject.transform.position.x, obj.transform.position.y, replacingObject.transform.position.z);
    }

    public void OnReplace(GameObject obj)
    {
        if (replacingItem is Weapon_DataScript weapon)
        {
            player.playerInventory.InventoryData.WeaponsList[Array.IndexOf(equipedWeaponsSlots, obj)] = weapon;
        }
        else if (replacingItem is ActiveItem_DataScript activeItem)
        {
            player.playerInventory.InventoryData.ActiveItemsList[Array.IndexOf(equipedActiveItemSlots, obj)] = activeItem;
        }
        UpdateAndDissableAll();
    }

    private void UpdateAndDissableAll()
    {
        canvas.SetActive(false);
        choosePanel.SetActive(false);
        replacePanel.SetActive(false);

        replacingItem = null;

        Cursor.lockState = CursorLockMode.Locked;
        player.ControllingData.Player.Get().SetActive(player);

        player.playerInventory.UpdateSlots();
        player.playerInventory.UpdateWeapon();
        player.playerInventory.UpdateActiveItemSlots();
        player.playerInventory.EnableAllPassives();
    }

    private Item_DataScript RollItem()
    {
        Item_DataScript[] itemPool = pools[UnityEngine.Random.Range(0, pools.Length)];

        if (itemPool.Length == 0) return null;

        return itemPool[UnityEngine.Random.Range(0, itemPool.Length)];
    }
}