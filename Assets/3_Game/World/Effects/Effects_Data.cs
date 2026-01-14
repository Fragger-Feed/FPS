using System;
using UnityEngine;

[CreateAssetMenu(fileName = "EffectsData", menuName = "Static/Effects/AllEffects")]
public class Effects_Data : ScriptableObject {
    public SerializableDictionary<string, Effect> EffectsData;
}



[Serializable]
public class Effect {
}