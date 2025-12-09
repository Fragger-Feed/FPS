using UnityEngine;

public class UseScript : MonoBehaviour
{
    [SerializeField] PlayerController playerController;

    public WeaponScript currentWeapon;
    void Update()
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
}
