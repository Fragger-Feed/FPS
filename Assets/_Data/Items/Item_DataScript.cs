using Unity.VisualScripting;
using UnityEngine;

public abstract class Item_DataScript : ScriptableObject
{
    public string itemName;
    [TextArea(3, 10)] public string description;
    public float rarity;
    public Sprite icon;
}