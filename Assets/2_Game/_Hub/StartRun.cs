using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartRun : InteractableOject
{
    private void Start()
    {
        Inventory_Data inventoryData = Object.FindFirstObjectByType<PlayerController>().playerInventory.InventoryData;
        for (int i = 0; i < inventoryData.WeaponsList.Length; i++)
        {
            inventoryData.WeaponsList[i] = null;
        }
        for (int i = 0; i < inventoryData.ActiveItemsList.Length; i++)
        {
            inventoryData.ActiveItemsList[i] = null;
        }
    }

    public override void Interact(PlayerController player)
    {
        SceneManager.LoadScene("Ectorris_Scene");
    }
}