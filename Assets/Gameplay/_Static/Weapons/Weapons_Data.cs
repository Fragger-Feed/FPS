using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapons_Data", menuName = "Static/Weapons/AllWeapons")]
public class Weapons_Data : ScriptableObject {
    public SerializableDictionary<string, Weapon> WeaponsData;
}



[Serializable]
public class Weapon {
    public Texture2D InventoryIcon;
}