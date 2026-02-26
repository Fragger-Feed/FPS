using UnityEngine;

[CreateAssetMenu(fileName = "AllWeapons_Data", menuName = "Prefabs/Weapons/All Weapons")]
public class AllWeapons_DataScript : ScriptableObject {
    public SerializableDictionary<string, Weapon_DataScript> WEAPONS_DATA;
}
