using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapons_Data", menuName = "Static/Weapons/AllWeapons")]
public class Weapons_Data : ScriptableObject {
    public Sprite PassiveInventoryIcon;
    public Sprite ActiveInventoryIcon;
    public GameObject behaviorPrefab;

}