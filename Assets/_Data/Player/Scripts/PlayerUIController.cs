using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIController : MonoBehaviour {
    // == Data ==
    [Header("Data")]
    public PlayerController PlayerController;

    // == Configs ==
    [Header("Configs")]
    public Menu CurrentMenuUI = Menu.ViewUI;
    public enum Menu {
        ViewUI,
        Inventory,
        Dialog
    }

    // == Objects ==
    [Header("Objects")]
    [Header("ViewUI")]
    public GameObject ViewUI;
    public List<GameObject> ViewUI_WeaponSlots = new List<GameObject>();
    public List<GameObject> ViewUI_ActiveItemSlots = new List<GameObject>();
    public List<GameObject> ViewUI_PassiveItemSlots = new List<GameObject>();
    public Slider ViewUI_Hp;

    [Header("Inventory")]
    public GameObject Inventory;
    public List<GameObject> Inventory_WeaponSlots = new List<GameObject>();
    public List<GameObject> Inventory_ActiveItemSlots = new List<GameObject>();
    public List<GameObject> Inventory_PassiveItemSlots = new List<GameObject>();



    // === Change ===
    public void ChangeMenu(Menu menuUI) {
        CurrentMenuUI = menuUI;

        ViewUI.SetActive(CurrentMenuUI == Menu.ViewUI);
        Inventory.SetActive(CurrentMenuUI == Menu.Inventory);
    }



    // === Updates ===
    public void UpdateCurrentMenu() {
        switch (CurrentMenuUI) {
            case Menu.ViewUI:
                UpdateViewUI();
                break;
            case Menu.Inventory:
                UpdateInventory();
                break;
        }
    }


    public void UpdateViewUI() {
        // HP
        ViewUI_Hp.maxValue = PlayerController.playerState.maxHealth;
        ViewUI_Hp.value = PlayerController.playerState.currentHealth;

        // Weapon
        for (int i = 0; i < Inventory_WeaponSlots.Count; i++) {
            Image image = Inventory_WeaponSlots[i].transform.GetChild(0).GetComponent<Image>();
            if (i == PlayerController.playerInventory.InventoryData.CurrentWeaponSlot) {
                Inventory_WeaponSlots[i].transform.localScale = PlayerController.playerInventory.iconScale;
                if (PlayerController.playerInventory.InventoryData.WeaponsList[i] != null) {
                    image.sprite = PlayerController.playerInventory.InventoryData.WeaponsList[i].ActiveInventoryIcon;
                    image.color = new Color(255, 255, 255, 1);
                }
                else {
                    image.sprite = null;
                    image.color = new Color(255, 255, 255, 0);
                }
            }
            else {
                Inventory_WeaponSlots[i].transform.localScale = Vector3.one;
                if (PlayerController.playerInventory.InventoryData.WeaponsList[i] != null) {
                    image.sprite = PlayerController.playerInventory.InventoryData.WeaponsList[i].PassiveInventoryIcon;
                    image.color = new Color(255, 255, 255, 1);
                }
                else {
                    image.sprite = null;
                    image.color = new Color(255, 255, 255, 0);
                }
            }
        }

    }

    public void UpdateInventory()  {
    }
}
