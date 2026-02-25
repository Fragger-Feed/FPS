using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Ectorris_Generator : MonoBehaviour
{
    private Vector3 startPos = Vector3.zero + new Vector3(0, 0, 300);
    private Vector3 direction = Vector3.forward;
    [SerializeField] private int mainPathLength = 20;
    [SerializeField] private int maxBranchLength = 5;
    [SerializeField] private int branchChance = 30;
    [SerializeField] private int branchCount = 10;
    [SerializeField] private List<BuildingNodes> buildingPrefabs = new List<BuildingNodes>();

    private void Start()
    {
        for (int i = 0; i < mainPathLength; i++)
        {
            var node = new BuildingNodes();
            node.position = startPos + direction * i * 10;
        }
    }
}

internal class BuildingNodes
{
    public Vector3 position;
    public Vector3 rotation;
    public Vector3 scale;
    public int Type;
    public List<BuildingNodes> Neighbors = new List<BuildingNodes>();
}