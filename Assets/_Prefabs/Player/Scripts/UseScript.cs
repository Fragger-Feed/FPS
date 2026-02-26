using UnityEngine;

public class UseScript : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;

    public WeaponScript currentWeapon;

    public GameObject[] activeItemSlots;

    [SerializeField] private float offset;
    [SerializeField] private Vector3 size;

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
            Collider[] hit = Physics.OverlapBox(playerController.CameraObj.transform.position + playerController.CameraObj.transform.forward * offset, size, playerController.CameraObj.transform.rotation);
            {   
                for (int i = 0; i < hit.Length; i++)
                {
                    if (hit[i].transform.parent == null)
                        continue;
                    InteractableOject interactable = hit[i].transform.parent.gameObject.GetComponent<InteractableOject>();
                    if (interactable != null)
                    {
                        interactable.Interact(playerController);
                        print(interactable.gameObject);
                        break;
                    }
                }
            }
        }
    }

    private void UseActive(int slot)
    {
        if (activeItemSlots[slot].transform.childCount > 0)
            activeItemSlots[slot].transform.GetChild(0).GetComponent<ActiveItem_Script>().UseActiveItem(playerController);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.matrix = Matrix4x4.TRS(playerController.CameraObj.transform.position + playerController.CameraObj.transform.forward * offset, playerController.CameraObj.transform.rotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, size * 2);
    }
}