using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon_Data", menuName = "Prefabs/Weapons/Weapon_Data")]
public class Weapon_DataScript : ScriptableObject
{
    public enum WeaponType
    {
        Melee,
        Ranged
    }

    public WeaponType weaponType;

    [Header("Icons")]
    public Sprite PassiveInventoryIcon;

    public Sprite ActiveInventoryIcon;

    [Header("Prefab"), Space(5)]
    public GameObject weaponPrefab;

    [Header("Ranged related variables"), Space(5)]
    public int damage;

    public float range;

    [Header("Melee related variables"), Space(5)]
    public float hitTime;

    public float hitCooldown;

    public Vector3 meleeHitPoint;
    public Vector3 meleeHitArea;

    public Vector3 projectileParryPoint;
    public Vector3 projectileParryArea;

    [Header("Etc"), Space(5)]
    public float fireRate;

    public float critMultiplier;
    public float critChance;
}