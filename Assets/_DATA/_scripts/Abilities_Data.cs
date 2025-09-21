using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "Abilities", menuName = "Data/Abilities")]
public class Abilities_Data : ScriptableObject {
    // == Data ==
    public SerializableDictionary<string, PlayerAbility> AllAbilities = new SerializableDictionary<string, PlayerAbility>();
}





// == Ability Class ==
public enum AbilityType { Passive, Active, TimeRepeated }
[Serializable]
public class PlayerAbility {
    public string Title;
    public string Description;

    public AbilityType Type;
}
