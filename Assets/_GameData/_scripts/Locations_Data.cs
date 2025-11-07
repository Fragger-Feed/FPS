using UnityEngine;

[CreateAssetMenu(fileName = "LocationsData", menuName = "Static/Locations/AllLocations")]
public class Locations_Data : ScriptableObject {
    public SerializableDictionary<string, LocationData> LocationsData;
}
