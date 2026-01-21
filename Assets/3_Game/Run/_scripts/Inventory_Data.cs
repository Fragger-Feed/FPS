using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Run", menuName = "Dinamic/Inventory")]
public class Inventory_Data : ScriptableObject
{
    public int CurrentWeaponSlot;
    public List<Weapons_Data> WeaponsList = new List<Weapons_Data>();

    public List<ActiveItem_Data> ActiveItemsList = new List<ActiveItem_Data>();
}