using System;
using System.Collections.Generic;
using UnityEngine;

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
    public bool altPath1;
    public bool altPath2;
    public bool isBranch;
    public List<BuildingNodes> Neighbors = new List<BuildingNodes>();
}

public class Ectorris_Generator : MonoBehaviour
{
    [SerializeField] private Vector3 startPos = Vector3.zero + new Vector3(0, 0, 300);

    [SerializeField] private int mainPathLength = 20;
    [SerializeField] private int altPathLenght = 69;
    [SerializeField] private int maxBranchLength = 5;
    [SerializeField] private int branchChance = 30;
    [SerializeField] private int branchCount = 10;

    [SerializeField] private List<Prefabs> buildingType = new List<Prefabs>();

    [SerializeField] private GameObject ziplinePrefab;
    [SerializeField] private GameObject branchTEST;
    [SerializeField] private GameObject bossRoom;

    private Vector3 position;

    private int dir = 1;
    private int prevDir = 1;
    private int idkHowToNameThisVariable = 1;

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

    private void Awake()
    {
        position = startPos;

        GenerateMainPath();
        GenerateAltPath();
        /*        GenerateBranches();*/
        InstantiateNodes();
    }

    private void GenerateMainPath()
    {
        for (int i = 0; i < mainPathLength; i++)
        {
            var node = new BuildingNodes();
            bool lastOne = i == mainPathLength - 1;
            node.Type = lastOne ? bossRoom : RandomType();

            if (i > 0)
            {
                node.Neighbors.Add(nodes[i - 1]);
                node.Neighbors[0].Neighbors.Add(node);

                position += CalcOffset(node.Neighbors[0].Type, node.Type, prevDir);
            }

            node.position = position;
            node.rotation = lastOne ? prevDir : dir;

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

    private void GenerateAltPath()
    {
        List<BuildingNodes> mainPath = nodes;
        bool succsesfullyGenerated = false;

        while (!succsesfullyGenerated)
        {
            BuildingNodes origin = mainPath[UnityEngine.Random.Range(10, mainPath.Count - 1)];

            Vector3 position = origin.position;
            int dir = (origin.rotation + 1 + 4) % 4;
            int prevDir = dir;
            idkHowToNameThisVariable = dir;

            BuildingNodes previousNode = origin;
            for (int i = 0; i < altPathLenght; i++)
            {
                var node = new BuildingNodes();
                bool lastOne = i == altPathLenght - 1;
                node.Type = lastOne ? bossRoom : RandomType();
                node.Neighbors.Add(previousNode);
                previousNode.Neighbors.Add(node);

                Transform prevOut = previousNode.Type.transform.Find("ZiplineLocationNorth");
                Transform nextIn = node.Type.transform.Find("ZiplineLocationSouth");

                if (prevOut == null || nextIn == null) break;

                position += CalcOffset(previousNode.Type, node.Type, prevDir);

                node.position = position;
                node.rotation = lastOne ? prevDir : dir;
                node.altPath1 = true;

                if (!DoesOverlap(node.Type, position, node.rotation))
                {
                    prevDir = dir;
                    dir = RandomDirection(node);
                    nodes.Add(node);
                    AddPlaced(node.Type, position, node.rotation);
                    previousNode = node;
                    succsesfullyGenerated = true;
                }
                else
                {
                    succsesfullyGenerated = false;
                    break;
                }
            }
        }
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
        int turnDir = UnityEngine.Random.Range(0, 2) == 0 ? -1 : 1;

        dir = (origin.rotation + turnDir + 4) % 4;
        prevDir = dir;
        idkHowToNameThisVariable = dir;

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

    private int RandomDirection(BuildingNodes node)
    {
        int roll = UnityEngine.Random.Range(0, 100);

        if (roll >= 70)
            idkHowToNameThisVariable += roll < 85 ? 1 : -1;

        if (node.altPath1)
        {
            idkHowToNameThisVariable = (idkHowToNameThisVariable < 1 || idkHowToNameThisVariable > 3) ? 2 : idkHowToNameThisVariable;
            return idkHowToNameThisVariable;
        }

        /*        if (node.altPath2)
                {
                    idkHowToNameThisVariable = (idkHowToNameThisVariable <)
                }*/

        if (!node.isBranch)
        {
            idkHowToNameThisVariable = (idkHowToNameThisVariable < 0 || idkHowToNameThisVariable > 2) ? 1 : idkHowToNameThisVariable;
            return idkHowToNameThisVariable;
        }

        return (idkHowToNameThisVariable + 4) % 4;
    }

    private GameObject RandomType()
    {
        int totalWeight = 0;
        foreach (var type in buildingType)
            totalWeight += type.weight;

        int roll = UnityEngine.Random.Range(0, totalWeight);
        int total = 0;

        foreach (var type in buildingType)
        {
            total += type.weight;
            if (roll < total)
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