using System.Collections.Generic;
using Unity.VisualScripting;
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

    private void Start()
    {
        UpdateSlots();
        UpdateWeapon();
    }

    private void Update()
    {
        int scroll = (int)PlayerController.ControllingData.Player.Scroll.ReadValue<float>();
        if (scroll != 0)  SelectWeaponSlot(scroll, false);
        int select = (int)PlayerController.ControllingData.Player.SelectWeapon.ReadValue<float>();
        if (select != 0) SelectWeaponSlot(select, true);
    }

    public void SelectWeaponSlot(int value, bool direct)
    {
        if (!direct)
        {
            InventoryData.CurrentWeaponSlot = (InventoryData.CurrentWeaponSlot + value + WeaponSlots.Count) % WeaponSlots.Count;
        }
        else
        {
            InventoryData.CurrentWeaponSlot = value - 1; 
        }
        UpdateSlots();
        UpdateWeapon();
    }

    void UpdateSlots()
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
                    image.color = new Color(255,255,255, 1);
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

    void UpdateWeapon()
    {
        Weapons_Data data = InventoryData.WeaponsList[InventoryData.CurrentWeaponSlot];
        if (currentWeapon != null)
        {
            Destroy(currentWeapon);
            currentWeapon = null;
            use.currentWeapon = null;
        }
        if(data == null)
            return;
        currentWeapon = Instantiate(data.weaponPrefab, WeaponParent.transform);
        use.currentWeapon = currentWeapon.GetComponent<WeaponScript>();
    }
}
