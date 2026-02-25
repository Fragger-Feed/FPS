using System;
using UnityEngine;

[CreateAssetMenu(fileName = "LocationsData", menuName = "Static/Locations/AllLocations")]
public class Locations_Data : ScriptableObject {
    public SerializableDictionary<string, Location> LocationsData;
}



[Serializable]public class Location {
    public int SceneID = 0;
}