using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Run", menuName = "Dinamic/Inventory")]

public class Inventory_Data : ScriptableObject {

    public int CurrentWeaponSlot;

    public List<string> WeaponsList = new List<string>();


}
