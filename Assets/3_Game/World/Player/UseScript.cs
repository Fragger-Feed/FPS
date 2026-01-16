using UnityEngine;

public class UseScript : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;

    public WeaponScript currentWeapon;

    private void Update()
    {
        if (currentWeapon != null && playerController.ControllingData.Player.PrimaryFire.WasPressedThisFrame())
        {
            currentWeapon.PrimaryFire(playerController.CameraObj.transform);
        }
        if (currentWeapon != null && playerController.ControllingData.Player.SecondaryFire.WasPressedThisFrame())
        {
            currentWeapon.SecondaryFire(playerController.CameraObj.transform);
        }
    }

    private void UseActive(int slot)
    {
    }
}