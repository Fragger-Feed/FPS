using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Composites;
using UnityEngine.Windows;
using static UnityEditor.Experimental.GraphView.GraphView;

public class PlayerInventory : MonoBehaviour
{
    public Inventory_Data InventoryData;
    public PlayerController Pcontroller;

    public List<GameObject> WeaponSlots = new List<GameObject>();


    void Start()
    {
        SelectWeaponSlot(InventoryData.CurrentWeaponSlot);
    }

    void Update()
    {
        float input = Pcontroller.ControllingData.Player.WeaponSlot_Plus.ReadValue<float>();
        if (input != 0) { SelectWeaponSlot(input); }
    }



    public void SelectWeaponSlot(float offset = 0) {
        InventoryData.CurrentWeaponSlot += (int)offset;
        if (InventoryData.CurrentWeaponSlot < 0) InventoryData.CurrentWeaponSlot = 3;
        if (InventoryData.CurrentWeaponSlot > 3) InventoryData.CurrentWeaponSlot = 0;

        for (int i = 0; i < WeaponSlots.Count; i++) {
            if (i != InventoryData.CurrentWeaponSlot) {
                WeaponSlots[i].transform.localScale = new Vector3(0.8f, 0.8f, 1);
            }
            else {
                WeaponSlots[i].transform.localScale = new Vector3(1, 1, 1);
            }
        }
        ;
    }
}
