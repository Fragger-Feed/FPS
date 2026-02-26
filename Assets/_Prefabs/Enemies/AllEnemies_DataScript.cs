using UnityEngine;

[CreateAssetMenu(fileName = "AllEnemies_Data", menuName = "Prefabs/Enemies/All Enemies")]
public class AllEnemies_DataScript : ScriptableObject {
    public SerializableDictionary<string, Enemy_DataScript> ENEMIES_DATA;
}
