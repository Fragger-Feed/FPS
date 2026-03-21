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
    private List<(Bounds bounds, BuildingNodes nodes)> placedBounds = new List<(Bounds, BuildingNodes)>();

    private string[] ziplinePoints = {
    "ZiplineLocationNorth",
    "ZiplineLocationSouth",
    "ZiplineLocationEast",
    "ZiplineLocationWest"
};

    [SerializeField] private Vector3 startPos = Vector3.zero + new Vector3(0, 0, 300);

    [SerializeField] private int mainPathLength = 69;
    [SerializeField] private int maxBranchLength = 69;
    [SerializeField] private int branchChance = 69;
    [SerializeField] private int branchCount = 69;

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
                node.Neighbours.Add(nodes[i - 1]);
            }

            nodes.Add(node);

            if (i > 0)
            {
                Transform prevOut = node.Neighbours[0].Type.transform.Find("ZiplineLocationNorth");
                Transform nextIn = node.Type.transform.Find("ZiplineLocationSouth");

                Vector3 target = new Vector3(0, 0, prevOut.localPosition.z + 50);
                Vector3 offset = target + new Vector3(0, 0, Mathf.Abs(nextIn.localPosition.z));

                Vector3 finalOffset = Quaternion.LookRotation(directions[prevDir]) * offset;

                position += finalOffset;
            }
            node.position = position;
            node.rotation = dir;

            AddPlaced(node.Type, position, node.rotation, node);

            prevDir = dir;
            dir = randomDirection(node);
        }

        var final = new BuildingNodes();
        final.Type = bossRoom;
        final.Neighbours.Add(nodes[nodes.Count - 1]);

        nodes.Add(final);

        Transform Fout = final.Neighbours[0].Type.transform.Find("ZiplineLocationNorth");
        Transform Fin = final.Type.transform.Find("ZiplineLocationSouth");

        Vector3 Ftarget = new Vector3(0, 0, Fout.localPosition.z + 50);
        Vector3 Foffset = Ftarget + new Vector3(0, 0, Mathf.Abs(Fin.localPosition.z));

        Vector3 lastOffset = Quaternion.LookRotation(directions[prevDir]) * Foffset;
        position += lastOffset;

        final.position = position;
        final.rotation = final.Neighbours[0].rotation;

        AddPlaced(final.Type, position, final.rotation, final);

        int branchesGenrated = 0;
        List<BuildingNodes> mainPath = nodes;
        for (int i = 0; i < mainPath.Count - 1; i++)
        {
            if (branchesGenrated >= branchCount) break;
            if (UnityEngine.Random.Range(0, 100) >= branchChance) continue;

            if (GenerateBranch(nodes[i]))
                branchesGenrated++;
        }

        InstantiateNodes();
    }

    private bool GenerateBranch(BuildingNodes origin)
    {
        int lenght = UnityEngine.Random.Range(1, maxBranchLength);
        int random = UnityEngine.Random.Range(0, 2) == 0 ? -1 : 1;
        dir = (origin.rotation + random + 4) % 4;
        prevDir = dir;
        idkHowToNameThisVariable = 0;
        Vector3 position = origin.position;
        BuildingNodes previousNode = origin;
        bool didAtleastOneGenerated = false;

        for (int j = 0; j < lenght; j++)
        {
            var node = new BuildingNodes();
            node.Type = RandomType();
            node.Neighbours.Add(previousNode);

            Transform prevOut = node.Neighbours[0].Type.transform.Find("ZiplineLocationNorth");
            Transform nextIn = node.Type.transform.Find("ZiplineLocationSouth");

            if (prevOut == null || nextIn == null) break;

            Vector3 target = new Vector3(0, 0, prevOut.localPosition.z + 50);
            Vector3 offset = target + new Vector3(0, 0, Mathf.Abs(nextIn.localPosition.z));

            Vector3 finalOffset = Quaternion.LookRotation(directions[prevDir]) * offset;

            position += finalOffset;

            node.position = position;
            node.rotation = dir;
            node.isBranch = true;
            BuildingNodes overlapingNode;
            if (!doesItOverlap(node.Type, position, node.rotation, out overlapingNode))
            {
                prevDir = dir;
                dir = randomDirection(node);
                nodes.Add(node);
                AddPlaced(node.Type, position, node.rotation, node);
                previousNode = node;
                didAtleastOneGenerated = true;
            }
            else
            {
                if (overlapingNode != null)
                    previousNode.Neighbours.Add(overlapingNode);
                break;
            }
        }
        return didAtleastOneGenerated;
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

    private bool doesItOverlap(GameObject prefab, Vector3 pos, int rotation, out BuildingNodes node)
    {
        Bounds newNodeBound = GetPrefabBounds(prefab, pos, rotation);
        newNodeBound.Expand(20);

        foreach (var existingBound in placedBounds)
            if (newNodeBound.Intersects(existingBound.bounds))
            {
                node = existingBound.nodes;
                return true;
            }
        node = null;
        return false;
    }

    private void AddPlaced(GameObject prefab, Vector3 pos, int rotation, BuildingNodes node)
    {
        Bounds bounds = GetPrefabBounds(prefab, pos, rotation);
        bounds.Expand(20);
        placedBounds.Add((bounds, node));
    }

    private (Transform from, Transform to) FindClosestPoints(Transform from, Transform to)
    {
        Transform closestFrom = null;
        Transform closestTo = null;
        float bestDis = float.MaxValue;
        foreach (string fromPoint in ziplinePoints)
        {
            Transform fPoint = from.Find(fromPoint);
            if (fPoint == null) continue;

            foreach (string toPoint in ziplinePoints)
            {
                Transform tPoint = to.Find(toPoint);
                if (tPoint == null) continue;

                float dist = Vector3.Distance(fPoint.position, tPoint.position);
                if (dist < bestDis)
                {
                    bestDis = dist;
                    closestFrom = fPoint;
                    closestTo = tPoint;
                }
            }
        }
        return (closestFrom, closestTo);
    }

    private void InstantiateNodes()
    {
        foreach (BuildingNodes node in nodes)
        {
            node.instance = Instantiate(node.Type, node.position, Quaternion.LookRotation(directions[node.rotation], Vector3.up), transform);
            if (node.Neighbours.Count == 0) continue;

            for (int i = 0; i < node.Neighbours.Count; i++)
            {
                GameObject neighbour = node.Neighbours[i].instance;

                if (neighbour == null) continue;

                var (ziplineOut, ziplineIn) = FindClosestPoints(neighbour.transform, node.instance.transform);

                if (ziplineIn != null && ziplineOut != null)
                {
                    GameObject zipline = Instantiate(ziplinePrefab, node.instance.transform);
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

    private int randomDirection(BuildingNodes node)
    {
        int weightedRandom = UnityEngine.Random.Range(0, 100);
        if (weightedRandom >= 70)
        {
            idkHowToNameThisVariable += weightedRandom < 85 ? 1 : -1;
        }
        if (!node.isBranch)
            return idkHowToNameThisVariable = idkHowToNameThisVariable < 0 || idkHowToNameThisVariable > 2 ? 1 : idkHowToNameThisVariable;

        return (idkHowToNameThisVariable + 4) % 4;
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

        foreach (var bound in placedBounds)
        {
            if (bound.bounds.extents == Vector3.zero) continue;

            Gizmos.DrawWireCube(bound.bounds.center, bound.bounds.size);
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
    public List<BuildingNodes> Neighbours = new List<BuildingNodes>();
}