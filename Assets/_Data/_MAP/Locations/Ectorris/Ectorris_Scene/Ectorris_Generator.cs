using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Ectorris_Generator : MonoBehaviour
{
    [SerializeField] private Vector3 startPos = Vector3.zero + new Vector3(0, 0, 300);
    private Vector3 position;
    private int dir = 1;
    private Vector3 prevDir;
    private Vector3[] directions = { Vector3.right, Vector3.forward, Vector3.left, Vector3.back };
    [SerializeField] private int mainPathLength = 20;
    [SerializeField] private int maxBranchLength = 5;
    [SerializeField] private int branchChance = 30;
    [SerializeField] private int branchCount = 10;
    [SerializeField] private List<prefabs> buildingType = new List<prefabs>();
    [SerializeField] private GameObject ziplinePrefab;
    [SerializeField] private List<BuildingNodes> nodes = new List<BuildingNodes>();
    [SerializeField] private GameObject branchTEST;
    private int idkHowToNameThisVariable = 1;

    private void Awake()
    {
        position = startPos;
        for (int i = 0; i < mainPathLength; i++)
        {
            var node = new BuildingNodes();

            node.Type = RandomType();

            if (i > 0)
            {
                node.Neighbors.Add(nodes[i - 1]);
            }

            nodes.Add(node);

            dir = randomDirection(node);
            if (i > 0)
            {
                Transform prevOut = node.Neighbors[0].Type.transform.Find("ZiplineLocationOut");
                Transform nextIn = node.Type.transform.Find("ZiplineLocationIn");

                Vector3 target = new Vector3(0, 0, prevOut.localPosition.z + 50);
                Vector3 offset = target + new Vector3(0, 0, Mathf.Abs(nextIn.localPosition.z));

                Vector3 finalOffset = Quaternion.LookRotation(directions[dir]) * offset;

                position += finalOffset;
            }
            node.position = position;
            node.rotation = dir;
        }

        int branchesGenrated = 0;
        List<BuildingNodes> mainPath = nodes;
        for (int i = 0; i < mainPath.Count; i++)
        {
            if (branchesGenrated >= branchCount) break;
            if (UnityEngine.Random.Range(0, 100) >= branchChance) continue;

            GenerateBranch(nodes[i]);

            branchesGenrated++;
        }

        InstantiateNodes();
    }

    private void GenerateBranch(BuildingNodes origin)
    {
        int lenght = UnityEngine.Random.Range(1, maxBranchLength);

        idkHowToNameThisVariable = origin.rotation + UnityEngine.Random.Range(1, 2) == 1 ? -1 : 1;
        if (idkHowToNameThisVariable < 0)
            idkHowToNameThisVariable = 3;
        if (idkHowToNameThisVariable > 3)
            idkHowToNameThisVariable = 0;
        Vector3 position = origin.position;
        BuildingNodes previousNode = origin;

        for (int j = 0; j < lenght; j++)
        {
            var node = new BuildingNodes();
            node.Type = branchTEST;
            node.Neighbors.Add(previousNode);

            Transform prevOut = node.Neighbors[0].Type.transform.Find("ZiplineLocationOut");
            Transform nextIn = node.Type.transform.Find("ZiplineLocationIn");

            Vector3 target = new Vector3(0, 0, prevOut.localPosition.z + 50);
            Vector3 offset = target + new Vector3(0, 0, Mathf.Abs(nextIn.localPosition.z));

            Vector3 finalOffset = Quaternion.LookRotation(directions[dir]) * offset;

            position += finalOffset;

            node.position = position;
            node.rotation = idkHowToNameThisVariable;
            node.isBranch = true;

            nodes.Add(node);
            previousNode = node;
        }
    }

    private void InstantiateNodes()
    {
        foreach (BuildingNodes node in nodes)
        {
            print(node.rotation);
            node.instance = Instantiate(node.Type, node.position, Quaternion.LookRotation(directions[node.rotation], Vector3.up), transform);

            if (node.Neighbors.Count == 0 || node.Neighbors[0] == null) continue;

            GameObject previous = node.Neighbors[0].instance;

            if (previous == null) continue;

            Transform ziplineIn = node.instance.transform.Find("ZiplineLocationIn");
            Transform ziplineOut = previous.transform.Find("ZiplineLocationOut");

            if (ziplineIn != null && ziplineOut != null)
            {
                GameObject zipline = Instantiate(ziplinePrefab, previous.transform);
                zipline.transform.position = ziplineOut.position;
                Transform pointA = zipline.transform.Find("PointA");
                Transform pointB = zipline.transform.Find("PointB");

                pointA.position = ziplineOut.position;
                pointB.position = ziplineIn.position;

                pointA.rotation = Quaternion.LookRotation(pointA.position - pointB.position);
                pointB.rotation = Quaternion.LookRotation(pointB.position - pointA.position);
            }
        }
    }

    private int randomDirection(BuildingNodes node)
    {
        int weightedRandom = UnityEngine.Random.Range(0, 100);
        if (weightedRandom >= 70)
        {
            idkHowToNameThisVariable += weightedRandom < 85 ? 1 : -1;
        }
        if (!node.isBranch)
            return idkHowToNameThisVariable = idkHowToNameThisVariable < 0 || idkHowToNameThisVariable > 2 ? 1 : idkHowToNameThisVariable;

        if (idkHowToNameThisVariable < 0)
            idkHowToNameThisVariable = 3;
        if (idkHowToNameThisVariable > 3)
            idkHowToNameThisVariable = 0;
        return idkHowToNameThisVariable;
    }

    private GameObject RandomType()
    {
        int totalWeight = 0;
        foreach (var type in buildingType)
        {
            totalWeight += type.weight;
        }

        int randomWeight = UnityEngine.Random.Range(0, totalWeight);

        int num = 0;
        foreach (var type in buildingType)
        {
            num += type.weight;
            if (randomWeight < num)
            {
                return type.prefab;
            }
        }
        return buildingType[0].prefab;
    }
}

[Serializable]
public class prefabs
{
    public string name;
    public GameObject prefab;
    public int weight;
}

internal class BuildingNodes
{
    public Vector3 position;
    public int rotation;
    public Vector3 scale;
    public GameObject Type;
    public GameObject instance;
    public bool isBranch;
    public List<BuildingNodes> Neighbors = new List<BuildingNodes>();
}