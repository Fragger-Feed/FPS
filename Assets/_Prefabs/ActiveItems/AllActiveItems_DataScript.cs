using UnityEngine;

[CreateAssetMenu(fileName = "AllActiveItems_Data", menuName = "Prefabs/Active Items/All Active Items")]

public class AllActiveItems_DataScript : ScriptableObject {
    public SerializableDictionary<string, ActiveItem_DataScript> ACTIVE_ITEMS_DATA;
}
