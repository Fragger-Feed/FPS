using JetBrains.Annotations;
using System;
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
    [SerializeField] private List<prefabs> buildingType = new List<prefabs>();
    [SerializeField] private GameObject ziplinePrefab;
    [SerializeField] private List<BuildingNodes> nodes = new List<BuildingNodes>();
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

            dir = randomDirection();
            if (i > 0)
            {
                Transform prevOut = node.Neighbors[0].Type.transform.Find("ZiplineLocationOut");
                Transform nextIn = node.Type.transform.Find("ZiplineLocationIn");

                Vector3 target = new Vector3(0, 0, prevOut.localPosition.z + 100);
                Vector3 offset = target + new Vector3(0, 0, Mathf.Abs(nextIn.localPosition.z));

                Vector3 finalOffset = Quaternion.LookRotation(dir) * offset;

                position += finalOffset;
            }
            node.position = position;
            node.rotation = dir;
        }

        for (int i = 0; i < nodes.Count; i++)
        {
            nodes[i].instance = Instantiate(nodes[i].Type, nodes[i].position, Quaternion.LookRotation(nodes[i].rotation, Vector3.up), transform);
            if (i > 0)
            {
                GameObject current = nodes[i].instance;
                GameObject previous = nodes[i - 1].instance;

                Transform ziplineIn = current.transform.Find("ZiplineLocationIn");
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
    }

    private Vector3 randomDirection()
    {
        int weightedRandom = UnityEngine.Random.Range(0, 100);
        if (weightedRandom >= 70)
        {
            idkHowToNameThisVariable += weightedRandom < 85 ? 1 : -1;
        }
        idkHowToNameThisVariable = idkHowToNameThisVariable < 0 || idkHowToNameThisVariable > 2 ? 1 : idkHowToNameThisVariable;
        return directions[idkHowToNameThisVariable];
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
    public Vector3 rotation;
    public Vector3 scale;
    public GameObject Type;
    public GameObject instance;
    public List<BuildingNodes> Neighbors = new List<BuildingNodes>();
}