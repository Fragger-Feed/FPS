using UnityEditor;
using UnityEngine;
[CreateAssetMenu(fileName = "LocationData", menuName = "Static/Locations/Data")]

public class LocationData : ScriptableObject {
    public int SceneID = 0;

    public int MaxParts;
    public int CurrentParts;
}
