using UnityEngine;

public class UseScript : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;

    public WeaponScript currentWeapon;

    public GameObject[] activeItemSlots;

    private void Update()
    {
        int activeSlot = 0;
        if (playerController.ControllingData.Player.UseArtifact.WasPressedThisFrame())
            activeSlot = (int)playerController.ControllingData.Player.UseArtifact.ReadValue<float>();
        if (currentWeapon != null && playerController.ControllingData.Player.PrimaryFire.WasPressedThisFrame())
        {
            currentWeapon.PrimaryFire(playerController.CameraObj.transform);
        }
        if (currentWeapon != null && playerController.ControllingData.Player.SecondaryFire.WasPressedThisFrame())
        {
            currentWeapon.SecondaryFire(playerController.CameraObj.transform);
        }
        if (activeSlot != 0)
        {
            UseActive(activeSlot - 1);
        }

        if (playerController.ControllingData.Player.Interact.WasPressedThisFrame())
        {
            RaycastHit hit;
            Physics.Raycast(playerController.CameraObj.transform.position, playerController.CameraObj.transform.forward, out hit, 3f);
            if (hit.collider != null)
            {
                InteractableOject interactable = hit.transform.parent.gameObject.GetComponent<InteractableOject>();
                if (interactable != null)
                {
                    interactable.Interact(playerController);
                }
            }
        }
    }

    private void UseActive(int slot)
    {
        if (activeItemSlots[slot].transform.childCount > 0)
            activeItemSlots[slot].transform.GetChild(0).GetComponent<ActiveItemScript>().UseItem(playerController);
    }
}