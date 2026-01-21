using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour
{
    public Weapons_Data Weapons;

    public Inventory_Data InventoryData;
    public PlayerController PlayerController;
    public UseScript use;

    public Vector3 iconScale;

    public List<GameObject> WeaponSlots = new List<GameObject>();
    public GameObject WeaponParent;
    public GameObject currentWeapon;

    public GameObject[] activeItemSlots;
    public GameObject[] activeItemSlotsUI;
    public GameObject[] pastItem;

    private void Start()
    {
        UpdateSlots();
        UpdateWeapon();
        UpdateActiveItemSlots();
    }

    private void Update()
    {
        UpdateActiveItemSlots();
        int scroll = (int)PlayerController.ControllingData.Player.Scroll.ReadValue<float>();
        if (scroll != 0) SelectWeaponSlot(scroll, false);
        int select = (int)PlayerController.ControllingData.Player.SelectWeapon.ReadValue<float>();
        if (select != 0) SelectWeaponSlot(select, true);
    }

    public void SelectWeaponSlot(int value, bool direct)
    {
        int previousSlot = InventoryData.CurrentWeaponSlot;
        if (!direct)
        {
            InventoryData.CurrentWeaponSlot = (InventoryData.CurrentWeaponSlot + value + WeaponSlots.Count) % WeaponSlots.Count;
        }
        else
        {
            InventoryData.CurrentWeaponSlot = value - 1;
        }
        if (previousSlot != InventoryData.CurrentWeaponSlot)
        {
            UpdateSlots();
            UpdateWeapon();
        }
    }

    private void UpdateSlots()
    {
        for (int i = 0; i < WeaponSlots.Count; i++)
        {
            Image image = WeaponSlots[i].transform.GetChild(0).GetComponent<Image>();
            if (i == InventoryData.CurrentWeaponSlot)
            {
                WeaponSlots[i].transform.localScale = iconScale;
                if (InventoryData.WeaponsList[i] != null)
                {
                    image.sprite = InventoryData.WeaponsList[i].ActiveInventoryIcon;
                    image.color = new Color(255, 255, 255, 1);
                }
                else
                {
                    image.sprite = null;
                    image.color = new Color(255, 255, 255, 0);
                }
            }
            else
            {
                WeaponSlots[i].transform.localScale = Vector3.one;
                if (InventoryData.WeaponsList[i] != null)
                {
                    image.sprite = InventoryData.WeaponsList[i].PassiveInventoryIcon;
                    image.color = new Color(255, 255, 255, 1);
                }
                else
                {
                    image.sprite = null;
                    image.color = new Color(255, 255, 255, 0);
                }
            }
        }
    }

    private void UpdateWeapon()
    {
        Weapons_Data data = InventoryData.WeaponsList[InventoryData.CurrentWeaponSlot];
        if (currentWeapon != null)
        {
            Destroy(currentWeapon);
            currentWeapon = null;
            use.currentWeapon = null;
        }
        if (data == null)
            return;
        currentWeapon = Instantiate(data.weaponPrefab, WeaponParent.transform);
        use.currentWeapon = currentWeapon.GetComponent<WeaponScript>();
    }

    private void UpdateActiveItemSlots()
    {
        for (int i = 0; i < activeItemSlots.Length; i++)
        {
            Transform currentItem = activeItemSlots[i].transform.childCount > 0
                ? activeItemSlots[i].transform.GetChild(0)
                : null;

            var newItem = InventoryData.ActiveItemsList[i];

            if (currentItem != null && newItem != null && pastItem[i] == newItem.script)
                continue;

            Image image = activeItemSlotsUI[i].transform.GetChild(0).GetComponent<Image>();

            if (newItem != null)
            {
                image.sprite = newItem.sprite;
                image.color = new Color(1, 1, 1, 1);

                if (currentItem != null)
                    Destroy(currentItem.gameObject);

                GameObject gc = Instantiate(newItem.script, activeItemSlots[i].transform);
                pastItem[i] = newItem.script;
            }
            else
            {
                image.sprite = null;
                image.color = new Color(1, 1, 1, 0);

                if (currentItem != null)
                    Destroy(currentItem.gameObject);

                pastItem[i] = null;
            }
        }
    }
}