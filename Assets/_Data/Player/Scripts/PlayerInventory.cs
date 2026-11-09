using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
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

    public GameObject inventory;
    public GameObject[] inventoryWeaponSlots;
    public RectTransform cursorImageObj;
    public Image cursorImage;

    //
    public GameObject[] activeItemSlots;

    public GameObject[] activeItemSlotsUI;
    public GameObject[] inventoryActiveItemSlotsUI;
    public GameObject[] pastItem;

    //
    public GameObject passiveItemParent;

    public GameObject passiveItemUIParent;
    public GameObject passiveItemUIPrefab;

    //
    public bool inInventory = false;

    public WeaponScript currentWeapon;
    public UseScript use;
    public Vector3 iconScale;

    // Item dragging
    private Weapon_DataScript movingWeapon;

    private ActiveItem_DataScript movingItem;

    private int indexOfTheMovingItem;
    private bool isWeapon;

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

        if (inInventory && PlayerController.ControllingData.Inventory.Select.WasPressedThisFrame())
        {
            HandleMouseDown();
        }
        if (inInventory && PlayerController.ControllingData.Inventory.Select.IsPressed())
        {
            if (movingWeapon != null || movingItem != null)
            {
                Vector2 pos = PlayerController.ControllingData.Inventory.MousePosition.ReadValue<Vector2>();
                cursorImageObj.position = new Vector3(pos.x, pos.y);
            }
        }
        if (inInventory && PlayerController.ControllingData.Inventory.Select.WasReleasedThisFrame())
        {
            HandleMouseUp();
        }
        // Open Inverntory
        if (PlayerController.ControllingData.Player.Inventory.WasPressedThisFrame() || PlayerController.ControllingData.Inventory.Inventory.WasPressedThisFrame())
        {
            inInventory = !inInventory;
            UpdateInventory();
            PlayerController.ControllingData.Player.Get().SetActive(!inInventory);
            Cursor.lockState = (inInventory) ? CursorLockMode.None : CursorLockMode.Locked;
            inventory.SetActive(inInventory);
        }
    }

    public void EnableAllPassives()
    {
        for (int i = 0; i < InventoryData.PassiveItemList.Length; i++)
        {
            GameObject item = Instantiate(InventoryData.PassiveItemList[i].script, passiveItemParent.transform);

            item.GetComponent<PassiveItem_Script>().OnEquip(PlayerController);
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

    public void HandleMouseDown()
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current) { position = Mouse.current.position.ReadValue() };

        List<RaycastResult> result = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, result);

        if (inInventory && result.Count > 0)
        {
            GameObject slot = result[0].gameObject;
            isWeapon = inventoryWeaponSlots.Contains(slot);
            bool contains = isWeapon || inventoryActiveItemSlotsUI.Contains(slot);

            if (contains)
            {
                int i = isWeapon ? Array.IndexOf(inventoryWeaponSlots, slot) : Array.IndexOf(inventoryActiveItemSlotsUI, slot);
                if (isWeapon && InventoryData.WeaponsList[i] == null) return;
                else if (!isWeapon && InventoryData.ActiveItemsList[i] == null) return;
                if (isWeapon)
                    movingWeapon = InventoryData.WeaponsList[i];
                else
                    movingItem = InventoryData.ActiveItemsList[i];
                indexOfTheMovingItem = i;
                if (isWeapon)
                    InventoryData.WeaponsList[i] = null;
                else
                    InventoryData.ActiveItemsList[i] = null;
                cursorImage.sprite = isWeapon ? movingWeapon.PassiveInventoryIcon : movingItem.sprite;
                cursorImage.color = new Color(255, 255, 255, 1);
                UpdateInventory();
                UpdateSlots();
            }
        }
    }

    public void HandleMouseUp()
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current) { position = Mouse.current.position.ReadValue() };

        List<RaycastResult> result = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, result);
        GameObject slot = result[0].gameObject;

        if (slot == null) return;

        if (movingWeapon == null && movingItem == null) return;

        bool contains = isWeapon ? inventoryWeaponSlots.Contains(slot) : inventoryActiveItemSlotsUI.Contains(slot);

        if (contains)
        {
            int i = isWeapon ? Array.IndexOf(inventoryWeaponSlots, slot) : Array.IndexOf(inventoryActiveItemSlotsUI, slot);

            if (isWeapon && InventoryData.WeaponsList[i] != null)
                InventoryData.WeaponsList[indexOfTheMovingItem] = InventoryData.WeaponsList[i];
            else if (!isWeapon && InventoryData.ActiveItemsList[i] != null)
                InventoryData.ActiveItemsList[indexOfTheMovingItem] = InventoryData.ActiveItemsList[i];

            if (isWeapon)
                InventoryData.WeaponsList[i] = movingWeapon;
            else
                InventoryData.ActiveItemsList[i] = movingItem;

            if (isWeapon && i == InventoryData.CurrentWeaponSlot) UpdateWeapon();
        }
        else
        {
            if (isWeapon)
                InventoryData.WeaponsList[indexOfTheMovingItem] = movingWeapon;
            else
                InventoryData.ActiveItemsList[indexOfTheMovingItem] = movingItem;
        }
        UpdateInventory();
        UpdateSlots();
        UpdateActiveItemSlots();
        if (isWeapon && indexOfTheMovingItem == InventoryData.CurrentWeaponSlot) UpdateWeapon();
        cursorImage.color = new Color(255, 255, 255, 0);
        cursorImage.sprite = null;
        movingWeapon = null;
        movingItem = null;
        isWeapon = false;
        indexOfTheMovingItem = -1;
    }

    private void UpdateInventory()
    {
        // Weapon slots
        for (int i = 0; i < WeaponSlots.Count; i++)
        {
            Image image = inventoryWeaponSlots[i].transform.GetChild(0).GetComponent<Image>();

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
        // Active slots
        for (int i = 0; i < activeItemSlots.Length; i++)
        {
            Image image = inventoryActiveItemSlotsUI[i].transform.GetChild(0).GetComponent<Image>();

            if (InventoryData.ActiveItemsList[i] != null)
            {
                image.sprite = InventoryData.ActiveItemsList[i].sprite;
                image.color = new Color(255, 255, 255, 1);
            }
            else
            {
                image.sprite = null;
                image.color = new Color(255, 255, 255, 0);
            }
        }
        // Passive slots
        foreach (Transform child in passiveItemUIParent.transform)
            Destroy(child.gameObject);

        for (int i = 0; i < InventoryData.PassiveItemList.Length; i++)
        {
            GameObject slot = Instantiate(passiveItemUIPrefab, passiveItemUIParent.transform);
            Image image = slot.transform.GetChild(0).GetChild(0).GetComponent<Image>();

            image.sprite = InventoryData.PassiveItemList[i].sprite;
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

    // To my future self that WILL forget why there's past item here:
    // It's so if that slot wasn't changed do nothing idk if it's the best way to handle this but at least it works
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