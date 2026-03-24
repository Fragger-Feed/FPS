using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
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
    public List<BuildingNodes> Neighbours = new List<BuildingNodes>();
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
    private List<(Bounds bounds, BuildingNodes nodes)> placedBounds = new List<(Bounds, BuildingNodes)>();

    private void Awake()
    {
        position = startPos;

        GenerateMainPath();
        GenerateAltPath();
        GenerateBranches();
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
                node.Neighbours.Add(nodes[i - 1]);

                position += CalcOffset(node.Neighbours[0].Type, node.Type, prevDir);
            }

            node.position = position;
            node.rotation = lastOne ? prevDir : dir;

            AddPlaced(node.Type, position, node.rotation, node);
            nodes.Add(node);

            prevDir = dir;
            dir = RandomDirection(node);
        }
    }

    private void GenerateAltPath()
    {
        List<BuildingNodes> mainPath = nodes;
        List<BuildingNodes> tempList = new List<BuildingNodes>();
        bool succsesfullyGenerated = false;
        int loopCount = 0;
        while (!succsesfullyGenerated && loopCount <= 100)
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
                node.Neighbours.Add(previousNode);

                Transform prevOut = previousNode.Type.transform.Find("ZiplineLocationNorth");
                Transform nextIn = node.Type.transform.Find("ZiplineLocationSouth");

                if (prevOut == null || nextIn == null) break;

                position += CalcOffset(previousNode.Type, node.Type, prevDir);

                node.position = position;
                node.rotation = lastOne ? prevDir : dir;
                node.altPath1 = true;

                if (!DoesItOverlap(node.Type, position, node.rotation))
                {
                    prevDir = dir;
                    dir = RandomDirection(node);
                    tempList.Add(node);
                    previousNode = node;
                    succsesfullyGenerated = true;
                }
                else
                {
                    tempList.Clear();
                    succsesfullyGenerated = false;
                    break;
                }
            }
            loopCount++;
        }
        if (loopCount == 100)
        {
            Debug.LogError("Fuck");
            return;
        }
        print(loopCount);
        foreach (BuildingNodes node in tempList)
        {
            AddPlaced(node.Type, node.position, node.rotation, node);
        }
        nodes.AddRange(tempList);
        tempList.Clear();
        succsesfullyGenerated = false;
        loopCount = 0;
        while (!succsesfullyGenerated && loopCount <= 100)
        {
            BuildingNodes origin = mainPath[UnityEngine.Random.Range(10, mainPath.Count - 1)];

            Vector3 position = origin.position;
            int dir = (origin.rotation - 1 + 4) % 4;
            int prevDir = dir;
            idkHowToNameThisVariable = dir;

            BuildingNodes previousNode = origin;
            for (int i = 0; i < altPathLenght; i++)
            {
                var node = new BuildingNodes();
                bool lastOne = i == altPathLenght - 1;
                node.Type = lastOne ? bossRoom : RandomType();
                node.Neighbours.Add(previousNode);

                Transform prevOut = previousNode.Type.transform.Find("ZiplineLocationNorth");
                Transform nextIn = node.Type.transform.Find("ZiplineLocationSouth");

                if (prevOut == null || nextIn == null) break;

                position += CalcOffset(previousNode.Type, node.Type, prevDir);

                node.position = position;
                node.rotation = lastOne ? prevDir : dir;
                node.altPath2 = true;

                if (!DoesItOverlap(node.Type, position, node.rotation))
                {
                    prevDir = dir;
                    dir = RandomDirection(node);
                    tempList.Add(node);
                    previousNode = node;
                    succsesfullyGenerated = true;
                }
                else
                {
                    tempList.Clear();
                    succsesfullyGenerated = false;
                    break;
                }
            }
            loopCount++;
        }
        foreach (BuildingNodes node in tempList)
        {
            AddPlaced(node.Type, node.position, node.rotation, node);
        }
        nodes.AddRange(tempList);
    }

    private void GenerateBranches()
    {
        int branchesGenerated = 0;
        List<BuildingNodes> mainPath = nodes;

        for (int i = 0; i < mainPath.Count - 1; i++)
        {
            if (branchesGenerated >= branchCount) break;
            if (UnityEngine.Random.Range(0, 100) >= branchChance) continue;
            print("ЖОПА");
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
            node.Neighbours.Add(previousNode);

            Transform prevOut = previousNode.Type.transform.Find("ZiplineLocationNorth");
            Transform nextIn = node.Type.transform.Find("ZiplineLocationSouth");

            if (prevOut == null || nextIn == null) break;

            branchPosition += CalcOffset(previousNode.Type, node.Type, prevDir);

            node.position = branchPosition;
            node.rotation = dir;
            node.isBranch = true;

            BuildingNodes overlapingNode;
            if (!DoesItOverlap(node.Type, branchPosition, node.rotation, out overlapingNode))
            {
                prevDir = dir;
                dir = RandomDirection(node);
                nodes.Add(node);
                AddPlaced(node.Type, branchPosition, node.rotation, overlapingNode);
                previousNode = node;
                didAtLeastOneGenerate = true;
            }
            else
            {
                print("ASS");
                if (overlapingNode != null || j > 0)
                    previousNode.Neighbours.Add(overlapingNode);
                break;
            }
        }

        return didAtLeastOneGenerate;
    }

    private void InstantiateNodes()
    {
        foreach (BuildingNodes node in nodes)
        {
            node.instance = Instantiate(
                node.Type,
                node.position,
                Quaternion.LookRotation(directions[node.rotation], Vector3.up),
                transform
            );

            if (node.Neighbours.Count == 0) continue;

            for (int i = 0; i < node.Neighbours.Count; i++)
            {
                GameObject neighbour = node.Neighbours[i].instance;
                print(i);
                if (neighbour == null) continue;

                var (ziplineOut, ziplineIn) = FindClosestZiplinePoints(neighbour.transform, node.instance.transform);

                if (ziplineOut != null && ziplineIn != null)
                    SpawnZipline(node.instance.transform, ziplineOut, ziplineIn);
            }
        }
    }

    private void SpawnZipline(Transform parent, Transform PointAPos, Transform PointBPos)
    {
        GameObject zipline = Instantiate(ziplinePrefab, parent);
        zipline.transform.position = PointAPos.position;

        Transform pointA = zipline.transform.Find("PointA");
        Transform pointB = zipline.transform.Find("PointB");

        pointA.position = PointAPos.position;
        pointB.position = PointBPos.position;

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

    private bool DoesItOverlap(GameObject prefab, Vector3 pos, int rotation, out BuildingNodes node)
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

    private bool DoesItOverlap(GameObject prefab, Vector3 pos, int rotation)
    {
        BuildingNodes FUck;
        return DoesItOverlap(prefab, pos, rotation, out FUck);
    }

    private void AddPlaced(GameObject prefab, Vector3 pos, int rotation, BuildingNodes node)
    {
        Bounds bounds = GetPrefabBounds(prefab, pos, rotation);
        bounds.Expand(20);
        placedBounds.Add((bounds, node));
    }

    private int RandomDirection(BuildingNodes node)
    {
        int roll = UnityEngine.Random.Range(0, 100);

        if (roll >= 70)
            idkHowToNameThisVariable += roll < 85 ? 1 : -1;

        idkHowToNameThisVariable = (idkHowToNameThisVariable + 4) % 4;

        if (node.altPath1)
        {
            return idkHowToNameThisVariable == 0 ? 2 : idkHowToNameThisVariable;
        }

        if (node.altPath2)
        {
            return idkHowToNameThisVariable == 2 ? 0 : idkHowToNameThisVariable;
        }

        if (!node.isBranch)
        {
            return idkHowToNameThisVariable == 3 ? 1 : idkHowToNameThisVariable;
        }

        return idkHowToNameThisVariable;
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

        foreach (var bound in placedBounds)
        {
            if (bound.bounds.extents == Vector3.zero) continue;

            Gizmos.DrawWireCube(bound.bounds.center, bound.bounds.size);
        }
    }
}