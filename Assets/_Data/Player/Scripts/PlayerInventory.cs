using System.Collections.Generic;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour
{
    // == Data ==
    [Header("Data")]
    public Weapon_DataScript WeaponsData;

    public Inventory_Data InventoryData;
    public PlayerController PlayerController;

    // == Objects ==
    [Header("Objects")]
    public List<GameObject> WeaponSlots = new List<GameObject>();

    public GameObject WeaponParent;

    public GameObject Inventory;
    public GameObject[] InventoryWeaponSlots;


    public GameObject[] activeItemSlots;
    public GameObject[] activeItemSlotsUI;
    public GameObject[] pastItem;

    //
    public bool inInventory = false;

    public WeaponScript currentWeapon;
    public UseScript use;
    public Vector3 iconScale;

    // === START ===
    private void Start()
    {
        UpdateSlots();
        UpdateWeapon();
        UpdateActiveItemSlots();
    }

    // === UPDATE ===
    // = Update =
    private void Update()
    {
        int scroll = (int)PlayerController.ControllingData.Player.Scroll.ReadValue<float>();
        if (scroll != 0) SelectWeaponSlot(scroll, false);
        int select = (int)PlayerController.ControllingData.Player.SelectWeapon.ReadValue<float>();
        if (select != 0) SelectWeaponSlot(select, true);


        if (inInventory && PlayerController.ControllingData.Inventory.Select.IsPressed())
        {
            print("Penis");
        }
        // Open Inverntory
        if (PlayerController.ControllingData.Player.Inventory.WasPressedThisFrame() || PlayerController.ControllingData.Inventory.Inventory.WasPressedThisFrame())
        {
            inInventory = !inInventory;
            UpdateInventory();
            PlayerController.ControllingData.Player.Get().SetActive(!inInventory);
            Cursor.lockState = (inInventory) ? CursorLockMode.None : CursorLockMode.Locked; 
            Inventory.SetActive(inInventory);
        }
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

    void UpdateInventory()
    {
        for (int i = 0; i < WeaponSlots.Count; i++)
        {
            Image image = InventoryWeaponSlots[i].transform.GetChild(0).GetComponent<Image>();

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

    private void UpdateWeapon()
    {
        Weapon_DataScript data = InventoryData.WeaponsList[InventoryData.CurrentWeaponSlot];
        if (currentWeapon != null)
        {
            Destroy(currentWeapon.gameObject);
            currentWeapon = null;
            use.currentWeapon = null;
        }
        if (data == null) return;
        currentWeapon = Instantiate(data.weaponPrefab, WeaponParent.transform).GetComponent<WeaponScript>();
        use.currentWeapon = currentWeapon;
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

public static class InputExtensions
{
    public static void SetActive(this UnityEngine.InputSystem.InputActionMap map, bool value)
    {
        if (value) map.Enable();
        else map.Disable();
    }
}