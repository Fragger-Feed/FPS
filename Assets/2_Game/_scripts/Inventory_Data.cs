using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Run", menuName = "Dinamic/Inventory")]
public class Inventory_Data : ScriptableObject
{
    public int CurrentWeaponSlot;
    public List<Weapon_DataScript> WeaponsList = new List<Weapon_DataScript>();

    public List<ActiveItem_DataScript> ActiveItemsList = new List<ActiveItem_DataScript>();

    public List<PassiveItem_DataScript> PassiveItemList = new List<PassiveItem_DataScript>();
}