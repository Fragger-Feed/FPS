using JetBrains.Annotations;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Ectorris_Generator : MonoBehaviour
{
    [SerializeField] private Vector3 startPos = Vector3.zero + new Vector3(0, 0, 300);
    private Vector3 position;
    private Vector3 dir = Vector3.forward;
    private Vector3 prevDir;
    private Vector3[] directions = { Vector3.right, Vector3.forward, Vector3.left };
    [SerializeField] private int mainPathLength = 20;
    [SerializeField] private int maxBranchLength = 5;
    [SerializeField] private int branchChance = 30;
    [SerializeField] private int branchCount = 10;
    [SerializeField] private SerializableDictionary<string, GameObject> buildingType;
    private List<BuildingNodes> nodes = new List<BuildingNodes>();
    private int idkHowToNameThisVariable = 1;

    private void Start()
    {
        position = startPos;
        for (int i = 0; i < mainPathLength; i++)
        {
            var node = new BuildingNodes();
            node.position = position;
            node.rotation = dir;
            node.Type = RandomType();

            if (i > 0)
            {
                node.Neighbors.Add(nodes[i - 1]);
            }

            nodes.Add(node);

            dir = randomDirection();
            position += dir * 100;
        }

        for (int i = 0; i < nodes.Count; i++)
        {
            Instantiate(nodes[i].Type, nodes[i].position, Quaternion.LookRotation(nodes[i].rotation, Vector3.up), transform);
        }
    }

    private Vector3 randomDirection() {
        int weightedRandom = Random.Range(0, 100);
        if (weightedRandom >= 70) {
            idkHowToNameThisVariable += weightedRandom < 85 ? 1 : -1;
        }
        idkHowToNameThisVariable = idkHowToNameThisVariable < 0 || idkHowToNameThisVariable > 2 ? 1 : idkHowToNameThisVariable;
        return directions[idkHowToNameThisVariable];
    }

    private GameObject RandomType()
    {
        int index = Random.Range(0, buildingType.Count);

        int i = 0;
        foreach (var pair in buildingType)
        {
            if (i == index)
            {
                return pair.Value;
            }
            i++;
        }
        return null;
    }
}

internal class BuildingNodes
{
    public Vector3 position;
    public Vector3 rotation;
    public Vector3 scale;
    public GameObject Type;
    public List<BuildingNodes> Neighbors = new List<BuildingNodes>();
}