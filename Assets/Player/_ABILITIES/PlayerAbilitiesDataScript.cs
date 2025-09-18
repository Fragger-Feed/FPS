using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerAbilitiesData", menuName = "Data/PlayerAbilitiesData")]
public class PlayerAbilitiesDataScript : ScriptableObject {
    // == Data ==
    public float AvailableMemory;
    public List<PlayerAbility> AllAbilities = new List<PlayerAbility>();

    // == Current Abilities ==
    public float OccupiedMemory;
    public List<PlayerAbility> CurrentAbilities = new List<PlayerAbility>();
    public bool AddAbility(PlayerAbility Ability) {
        if (OccupiedMemory + Ability.Size <= AvailableMemory && !CurrentAbilities.Contains(Ability)) {
            OccupiedMemory += Ability.Size;
            CurrentAbilities.Add(Ability);
            return true;
        }
        return false;
    }
    public bool DeleteAbility(PlayerAbility Ability) {
        if (CurrentAbilities.Contains(Ability)) {
            OccupiedMemory -= Ability.Size;
            CurrentAbilities.Remove(Ability);
            return true;
        }
        return false;
    }
}





// == Ability Class ==
public enum AbilityType { Passive, Active }
[Serializable]
public class PlayerAbility {
    public string Title;
    public string Description;

    public float Size;
    public AbilityType Type;
}
