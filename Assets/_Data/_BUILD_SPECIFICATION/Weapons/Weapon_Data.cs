using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon_Data", menuName = "Prefabs/Weapons/Weapon_Data")]
public class Weapon_DataScript : ScriptableObject
{
    public Sprite PassiveInventoryIcon;
    public Sprite ActiveInventoryIcon;

    public GameObject weaponPrefab;

    public int damage;
    public float range;
    public float fireRate;
    public float critMultiplier;
    public float critChance;
}