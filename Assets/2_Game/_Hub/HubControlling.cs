using UnityEngine;
using UnityEngine.SceneManagement;

public class HubControlling : MonoBehaviour
{

    // === START ===
    private void Start() {

        // Clear Inventory
        Inventory_Data inventoryData = Object.FindFirstObjectByType<PlayerController>().playerInventory.InventoryData;
        for (int i = 0; i < inventoryData.WeaponsList.Count; i++) { inventoryData.WeaponsList[i] = null; }
        for (int i = 0; i < inventoryData.ActiveItemsList.Count; i++) { inventoryData.ActiveItemsList[i] = null; }
    }


    // Start Run
    public void StartRun() {
        SceneManager.LoadScene("Ectorris_Scene");
    }
}
