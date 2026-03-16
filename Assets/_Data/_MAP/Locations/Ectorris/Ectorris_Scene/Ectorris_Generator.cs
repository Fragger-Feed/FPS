using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Ectorris_Generator : MonoBehaviour
{
    private Vector3 position;

    private int dir = 1;
    private int prevDir = 1;
    private int idkHowToNameThisVariable = 1;

    private Vector3[] directions = { Vector3.right, Vector3.forward, Vector3.left, Vector3.back };
    private List<BuildingNodes> nodes = new List<BuildingNodes>();
    private List<Bounds> placedBounds = new List<Bounds>();

    [SerializeField] private Vector3 startPos = Vector3.zero + new Vector3(0, 0, 300);

    [SerializeField] private int mainPathLength = 20;
    [SerializeField] private int maxBranchLength = 5;
    [SerializeField] private int branchChance = 30;
    [SerializeField] private int branchCount = 10;

    [SerializeField] private List<prefabs> buildingType = new List<prefabs>();

    [SerializeField] private GameObject ziplinePrefab;
    [SerializeField] private GameObject branchTEST;
    [SerializeField] private GameObject bossRoom;

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

            if (i > 0)
            {
                Transform prevOut = node.Neighbors[0].Type.transform.Find("ZiplineLocationOut");
                Transform nextIn = node.Type.transform.Find("ZiplineLocationIn");

                Vector3 target = new Vector3(0, 0, prevOut.localPosition.z + 50);
                Vector3 offset = target + new Vector3(0, 0, Mathf.Abs(nextIn.localPosition.z));

                Vector3 finalOffset = Quaternion.LookRotation(directions[prevDir]) * offset;

                position += finalOffset;
            }
            node.position = position;
            node.rotation = dir;

            AddedPlaced(node.Type, position, node.rotation);

            prevDir = dir;
            dir = randomDirection(node);
        }

        var final = new BuildingNodes();
        final.Type = bossRoom;
        final.Neighbors.Add(nodes[nodes.Count - 1]);

        nodes.Add(final);

        Transform Fout = final.Neighbors[0].Type.transform.Find("ZiplineLocationOut");
        Transform Fin = final.Type.transform.Find("ZiplineLocationIn");

        Vector3 Ftarget = new Vector3(0, 0, Fout.localPosition.z + 50);
        Vector3 Foffset = Ftarget + new Vector3(0, 0, Mathf.Abs(Fin.localPosition.z));

        Vector3 lastOffset = Quaternion.LookRotation(directions[prevDir]) * Foffset;
        position += lastOffset;

        final.position = position;
        final.rotation = final.Neighbors[0].rotation;

        AddedPlaced(final.Type, position, final.rotation);

        int branchesGenrated = 0;
        List<BuildingNodes> mainPath = nodes;
        for (int i = 0; i < mainPath.Count - 1; i++)
        {
            if (branchesGenrated >= branchCount) break;
            if (UnityEngine.Random.Range(0, 100) >= branchChance) continue;

            GenerateBranch(nodes[i]);

            branchesGenrated++;
        }

        InstantiateNodes();
    }

    private bool GenerateBranch(BuildingNodes origin)
    {
        int lenght = UnityEngine.Random.Range(1, maxBranchLength);
        int random = UnityEngine.Random.Range(0, 2) == 0 ? -1 : 1;
        idkHowToNameThisVariable = (origin.rotation + random + 4) % 4;
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

            Vector3 finalOffset = Quaternion.LookRotation(directions[idkHowToNameThisVariable]) * offset;

            position += finalOffset;

            node.position = position;
            node.rotation = idkHowToNameThisVariable;
            node.isBranch = true;

            nodes.Add(node);

            AddedPlaced(node.Type, position, node.rotation);

            previousNode = node;
        }

        return true;
    }

    private Bounds GetPrefabBounds(GameObject prefab, Vector3 pos, int rotation)
    {
        GameObject temp = Instantiate(prefab, pos, Quaternion.LookRotation(directions[rotation]));
        temp.hideFlags = HideFlags.HideAndDontSave;

        Renderer[] renderers = temp.GetComponentsInChildren<Renderer>();

        if (renderers.Length == 0)
        {
            DestroyImmediate(temp);
            return new Bounds(pos, Vector3.zero);
        }

        Bounds bounds = renderers[0].bounds;
        foreach (Renderer r in renderers)
        {
            bounds.Encapsulate(r.bounds);
        }

        DestroyImmediate(temp);
        return bounds;
    }

    private bool doesItOverlap(GameObject prefab, Vector3 pos, int rotation)
    {
        Bounds newNodeBound = GetPrefabBounds(prefab, pos, rotation);
        newNodeBound.Expand(20);

        foreach (Bounds existingBound in placedBounds)
            if (newNodeBound.Intersects(existingBound))
                return true;

        return false;
    }

    private void AddedPlaced(GameObject prefab, Vector3 pos, int rotation)
    {
        Bounds bounds = GetPrefabBounds(prefab, pos, rotation);
        bounds.Expand(20);
        placedBounds.Add(bounds);
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

    private void OnDrawGizmos()
    {
        if (nodes == null) return;

        foreach (Bounds bound in placedBounds)
        {
            if (bound.extents == Vector3.zero) continue;

            Gizmos.DrawWireCube(bound.center, bound.size);
        }
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