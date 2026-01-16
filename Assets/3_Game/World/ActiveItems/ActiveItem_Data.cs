using UnityEngine;

[CreateAssetMenu(fileName = "ActiveItem_Data", menuName = "Static/ActiveItem_Data")]
public class ActiveItem_Data : ScriptableObject
{
    public string itemName;
    public string description;

    public Sprite sprite;

    public ActiveItemScript script;
}