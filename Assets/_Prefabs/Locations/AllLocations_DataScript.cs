using System;
using UnityEngine;

[CreateAssetMenu(fileName = "AllLocations_Data", menuName = "Prefabs/Locations/All Locations")]
public class AllLocations_DataScript : ScriptableObject {
    public SerializableDictionary<string, Location_DataScript> LOCATIONS_DATA;
}

