using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "Abilities", menuName = "Data/Abilities")]
public class Abilities_Data : ScriptableObject {
    // == Data ==
    public float AvailableMemory;
    public SerializableDictionary<string, PlayerAbility> AllAbilities = new SerializableDictionary<string, PlayerAbility>();

    // == Current Abilities ==
    public float OccupiedMemory;
    public List<string> CurrentAbilities = new List<string>();
}





// == Ability Class ==
public enum AbilityType { Passive, Active }
[Serializable]
public class PlayerAbility {
    public string Title;
    public string Description;

    public float Size;
    public AbilityType Type;

    public Texture2D FullTexture;
    public Texture2D SmallTexture;
}
