using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Localization.Plugins.XLIFF.V12;
using UnityEngine;
using static Ectorris_Generator;

[Serializable]
public class Spawnables
{
    public string name;
    public GameObject prefab;
    public int weight;
    [Range(0f, 1f)] public float minSpawnDificulty = 0;
}

[Serializable]
public class RoomContentCategory
{
    public string name;
    public string spawnPointPrefix;
    public List<Spawnables> spawnPool = new List<Spawnables>();
    public Vector2Int spawnAmountRange = new Vector2Int(1, 3);
}

[Serializable]
public class RoomTypeContent
{
    public RoomType roomType;
    public List<RoomContentCategory> categoryList = new List<RoomContentCategory>();
}

public class BuildingContent : MonoBehaviour
{
    [SerializeField] private List<RoomTypeContent> roomTypeContents = new List<RoomTypeContent>();
    private Dictionary<string, List<Transform>> spawnpointByCathegoty = new Dictionary<string, List<Transform>>();

    private List<GameObject> spawnedObjects = new List<GameObject>();

    private Color[] gizmosColors = { Color.red, Color.green, Color.blue };

    private bool populated = false;
    private bool activated = false;

    private void Awake()
    {
        spawnpointByCathegoty.Clear();

        List<RoomContentCategory> allCategories = roomTypeContents.SelectMany(rtc => rtc.categoryList).ToList();

        foreach (Transform child in GetComponentInChildren<Transform>(true))
        {
            if (child == null) continue;

            foreach (RoomContentCategory category in allCategories)
            {
                if (string.IsNullOrEmpty(category.spawnPointPrefix)) continue;
                if (!child.name.StartsWith(category.spawnPointPrefix)) continue;

                if (!spawnpointByCathegoty.TryGetValue(category.name, out List<Transform> list))
                {
                    list = new List<Transform>();
                    spawnpointByCathegoty[category.name] = list;
                }
                list.Add(child);
            }
        }
    }

    /// <summary>
    /// My ass itches
    /// </summary>
    public void Populate(RoomType roomType, float dificulty)
    {
        if (populated) return;
        populated = true;

        dificulty = Mathf.Clamp01(dificulty);

        RoomTypeContent content = roomTypeContents.FirstOrDefault(rtc => rtc.roomType == roomType);
        if (content == null) return;

        foreach (RoomContentCategory roomContent in content.categoryList)
        {
            List<Transform> spawnPoints = spawnpointByCathegoty.TryGetValue(roomContent.name, out var lists) ? lists : new List<Transform>();

            int count = Mathf.RoundToInt(Mathf.Lerp(roomContent.spawnAmountRange.x, roomContent.spawnAmountRange.y, dificulty));

            SpawnFromPool(roomContent.spawnPool, spawnPoints, count, dificulty);
        }
    }

    private void SpawnFromPool(List<Spawnables> spawnPool, List<Transform> spawnpoints, int spawnAmount, float difficulty)
    {
        if (spawnPool == null || spawnPool.Count == 0 || spawnpoints.Count == 0 || spawnAmount == 0) return;

        List<Spawnables> spawnList = spawnPool.Where(spawn => spawn.minSpawnDificulty <= difficulty).ToList();
        if (spawnList.Count == 0) return;

        spawnAmount = Mathf.Min(spawnAmount, spawnpoints.Count);

        List<Transform> availablePoints = new List<Transform>(spawnpoints);

        for (int i = 0; i < spawnAmount; i++)
        {
            if (availablePoints.Count == 0) break;

            int pointIndex = UnityEngine.Random.Range(0, availablePoints.Count);
            Transform spawnpoint = availablePoints[pointIndex];
            availablePoints.RemoveAt(pointIndex);

            GameObject prefab = PickPrefab(spawnList);
            if (prefab == null) continue;

            GameObject instance = Instantiate(prefab, spawnpoint.position, spawnpoint.rotation, transform);
            //instance.SetActive(false);
            spawnedObjects.Add(instance);
        }
    }

    private GameObject PickPrefab(List<Spawnables> spawnPool)
    {
        int totalWeight = spawnPool.Sum(e => e.weight);
        if (totalWeight <= 0) return spawnPool[0].prefab;

        int roll = UnityEngine.Random.Range(0, totalWeight);
        int total = 0;

        foreach (Spawnables spawn in spawnPool)
        {
            total += spawn.weight;
            if (roll < total)
                return spawn.prefab;
        }

        return spawnPool[0].prefab;
    }

    private void OnDrawGizmos()
    {
        int index = 0;
        foreach (var kvp in spawnpointByCathegoty)
        {
            Gizmos.color = gizmosColors[index];
            foreach (Transform spawnpoint in kvp.Value)

                if (spawnpoint != null) Gizmos.DrawSphere(spawnpoint.position, 1);
            index++;
        }
    }
}