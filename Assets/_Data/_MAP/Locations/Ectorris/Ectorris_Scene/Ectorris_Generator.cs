using System;
using System.Collections.Generic;
using UnityEngine;

// ─────────────────────────────────────────────
// Data Classes
// ─────────────────────────────────────────────

[Serializable]
public class Prefabs
{
    public string name;
    public GameObject prefab;
    public int weight;
}

public class BuildingNodes
{
    public Vector3 position;
    public int rotation;
    public Vector3 scale;
    public GameObject Type;
    public GameObject instance;
    public bool isBranch;
    public List<BuildingNodes> Neighbors = new List<BuildingNodes>();
}

// ─────────────────────────────────────────────
// Generator
// ─────────────────────────────────────────────

public class Ectorris_Generator : MonoBehaviour
{
    // ── Serialized Settings ──────────────────

    [SerializeField] private Vector3 startPos = Vector3.zero + new Vector3(0, 0, 300);

    [SerializeField] private int mainPathLength = 20;
    [SerializeField] private int maxBranchLength = 5;
    [SerializeField] private int branchChance = 30;
    [SerializeField] private int branchCount = 10;

    [SerializeField] private List<Prefabs> buildingType = new List<Prefabs>();

    [SerializeField] private GameObject ziplinePrefab;
    [SerializeField] private GameObject branchTEST;
    [SerializeField] private GameObject bossRoom;

    // ── Private State ────────────────────────

    private Vector3 position;

    private int dir = 1;
    private int prevDir = 1;
    private int directionBias = 1; // Controls weighted direction drift (was idkHowToNameThisVariable)

    private readonly Vector3[] directions = { Vector3.right, Vector3.forward, Vector3.left, Vector3.back };

    private readonly string[] ziplinePoints =
    {
        "ZiplineLocationNorth",
        "ZiplineLocationSouth",
        "ZiplineLocationEast",
        "ZiplineLocationWest"
    };

    private List<BuildingNodes> nodes = new List<BuildingNodes>();
    private List<Bounds> placedBounds = new List<Bounds>();

    // ─────────────────────────────────────────
    // Unity Lifecycle
    // ─────────────────────────────────────────

    private void Awake()
    {
        position = startPos;

        GenerateMainPath();
        GenerateBossRoom();
        GenerateBranches();
        InstantiateNodes();
    }

    // ─────────────────────────────────────────
    // Generation
    // ─────────────────────────────────────────

    private void GenerateMainPath()
    {
        for (int i = 0; i < mainPathLength; i++)
        {
            var node = new BuildingNodes();
            node.Type = RandomType();

            if (i > 0)
            {
                node.Neighbors.Add(nodes[i - 1]);
                node.Neighbors[0].Neighbors.Add(node);

                position += CalcOffset(node.Neighbors[0].Type, node.Type, prevDir);
            }

            node.position = position;
            node.rotation = dir;

            AddPlaced(node.Type, position, node.rotation);
            nodes.Add(node);

            prevDir = dir;
            dir = RandomDirection(node);
        }
    }

    private void GenerateBossRoom()
    {
        var final = new BuildingNodes();
        final.Type = bossRoom;
        final.Neighbors.Add(nodes[nodes.Count - 1]);
        final.Neighbors[0].Neighbors.Add(final);

        position += CalcOffset(final.Neighbors[0].Type, final.Type, prevDir);

        final.position = position;
        final.rotation = final.Neighbors[0].rotation;

        AddPlaced(final.Type, position, final.rotation);
        nodes.Add(final);
    }

    private void GenerateBranches()
    {
        int branchesGenerated = 0;
        List<BuildingNodes> mainPath = nodes;

        for (int i = 0; i < mainPath.Count - 1; i++)
        {
            if (branchesGenerated >= branchCount) break;
            if (UnityEngine.Random.Range(0, 100) >= branchChance) continue;

            if (GenerateBranch(nodes[i]))
                branchesGenerated++;
        }
    }

    private bool GenerateBranch(BuildingNodes origin)
    {
        int length = UnityEngine.Random.Range(1, maxBranchLength);
        int turnSign = UnityEngine.Random.Range(0, 2) == 0 ? -1 : 1;

        dir = (origin.rotation + turnSign + 4) % 4;
        prevDir = dir;
        directionBias = 0;

        Vector3 branchPosition = origin.position;
        BuildingNodes previousNode = origin;
        bool didAtLeastOneGenerate = false;

        for (int j = 0; j < length; j++)
        {
            var node = new BuildingNodes();
            node.Type = RandomType();
            node.Neighbors.Add(previousNode);
            previousNode.Neighbors.Add(node);

            Transform prevOut = previousNode.Type.transform.Find("ZiplineLocationNorth");
            Transform nextIn = node.Type.transform.Find("ZiplineLocationSouth");

            if (prevOut == null || nextIn == null) break;

            branchPosition += CalcOffset(previousNode.Type, node.Type, prevDir);

            node.position = branchPosition;
            node.rotation = dir;
            node.isBranch = true;

            if (!DoesOverlap(node.Type, branchPosition, node.rotation))
            {
                prevDir = dir;
                dir = RandomDirection(node);
                nodes.Add(node);
                AddPlaced(node.Type, branchPosition, node.rotation);
                previousNode = node;
                didAtLeastOneGenerate = true;
            }
            else
            {
                break;
            }
        }

        return didAtLeastOneGenerate;
    }

    // ─────────────────────────────────────────
    // Instantiation
    // ─────────────────────────────────────────

    private void InstantiateNodes()
    {
        foreach (BuildingNodes node in nodes)
        {
            print(node.rotation);
            node.instance = Instantiate(
                node.Type,
                node.position,
                Quaternion.LookRotation(directions[node.rotation], Vector3.up),
                transform
            );

            if (node.Neighbors.Count == 0 || node.Neighbors[0] == null) continue;

            GameObject previous = node.Neighbors[0].instance;
            if (previous == null) continue;

            var (ziplineOut, ziplineIn) = FindClosestZiplinePoints(previous.transform, node.instance.transform);

            if (ziplineOut != null && ziplineIn != null)
                SpawnZipline(previous.transform, ziplineOut, ziplineIn);
        }
    }

    private void SpawnZipline(Transform parent, Transform pointASource, Transform pointBSource)
    {
        GameObject zipline = Instantiate(ziplinePrefab, parent);
        zipline.transform.position = pointASource.position;

        Transform pointA = zipline.transform.Find("PointA");
        Transform pointB = zipline.transform.Find("PointB");

        pointA.position = pointASource.position;
        pointB.position = pointBSource.position;

        pointA.rotation = Quaternion.LookRotation(pointA.position - pointB.position);
        pointB.rotation = Quaternion.LookRotation(pointB.position - pointA.position);
    }

    // ─────────────────────────────────────────
    // Positioning Helpers
    // ─────────────────────────────────────────

    /// <summary>Calculates the world-space offset to place the next building after the previous one.</summary>
    private Vector3 CalcOffset(GameObject prevPrefab, GameObject nextPrefab, int facingDir)
    {
        Transform prevOut = prevPrefab.transform.Find("ZiplineLocationNorth");
        Transform nextIn = nextPrefab.transform.Find("ZiplineLocationSouth");

        Vector3 target = new Vector3(0, 0, prevOut.localPosition.z + 50);
        Vector3 offset = target + new Vector3(0, 0, Mathf.Abs(nextIn.localPosition.z));

        return Quaternion.LookRotation(directions[facingDir]) * offset;
    }

    private (Transform from, Transform to) FindClosestZiplinePoints(Transform from, Transform to)
    {
        Transform closestFrom = null;
        Transform closestTo = null;
        float bestDist = float.MaxValue;

        foreach (string fromPoint in ziplinePoints)
        {
            Transform fPoint = from.Find(fromPoint);
            if (fPoint == null) continue;

            foreach (string toPoint in ziplinePoints)
            {
                Transform tPoint = to.Find(toPoint);
                if (tPoint == null) continue;

                float dist = Vector3.Distance(fPoint.position, tPoint.position);
                if (dist < bestDist)
                {
                    bestDist = dist;
                    closestFrom = fPoint;
                    closestTo = tPoint;
                }
            }
        }

        return (closestFrom, closestTo);
    }

    // ─────────────────────────────────────────
    // Bounds & Overlap
    // ─────────────────────────────────────────

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
            bounds.Encapsulate(r.bounds);

        DestroyImmediate(temp);
        return bounds;
    }

    private bool DoesOverlap(GameObject prefab, Vector3 pos, int rotation)
    {
        Bounds newBounds = GetPrefabBounds(prefab, pos, rotation);
        newBounds.Expand(20);

        foreach (Bounds existing in placedBounds)
            if (newBounds.Intersects(existing))
                return true;

        return false;
    }

    private void AddPlaced(GameObject prefab, Vector3 pos, int rotation)
    {
        Bounds bounds = GetPrefabBounds(prefab, pos, rotation);
        bounds.Expand(20);
        placedBounds.Add(bounds);
    }

    // ─────────────────────────────────────────
    // Randomisation
    // ─────────────────────────────────────────

    private int RandomDirection(BuildingNodes node)
    {
        int roll = UnityEngine.Random.Range(0, 100);

        if (roll >= 70)
            directionBias += roll < 85 ? 1 : -1;

        if (!node.isBranch)
        {
            directionBias = (directionBias < 0 || directionBias > 2) ? 1 : directionBias;
            return directionBias;
        }

        return (directionBias + 4) % 4;
    }

    private GameObject RandomType()
    {
        int totalWeight = 0;
        foreach (var type in buildingType)
            totalWeight += type.weight;

        int roll = UnityEngine.Random.Range(0, totalWeight);
        int cumulative = 0;

        foreach (var type in buildingType)
        {
            cumulative += type.weight;
            if (roll < cumulative)
                return type.prefab;
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