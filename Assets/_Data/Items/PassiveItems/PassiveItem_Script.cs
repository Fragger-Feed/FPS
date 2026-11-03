using UnityEngine;

public abstract class PassiveItem_Script : MonoBehaviour
{
    private PlayerController player;

    public virtual void OnEquip(PlayerController player)
    {
    }

    public virtual void OnUnequip(PlayerController player)
    {
    }
}