using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;

public class Ectorris_MapController : MonoBehaviour
{
    // == Checks ==
    public List<Vector2Int> MapChecks = new List<Vector2Int>() { };
    Vector2Int boss = Vector2Int.zero;
    Vector2Int toLoc1 = Vector2Int.zero;
    Vector2Int toLoc2 = Vector2Int.zero;
    Vector2Int toLoc3 = Vector2Int.zero;

    // == Rooms ==
    [Serializable]
    class RoomType {
        public bool END_ROOM;
        public List<string> Form = new List<string>();
        public List<GameObject> RoomPrefabs = new List<GameObject>();
    }
    [SerializeField] List<RoomType> RoomPrefabs = new List<RoomType>() { };




    public GameObject Prefab;
    public float cellSize = 10f;



    // ==== START ====
    void Start() {

        // == Checks ==
        // Start-way
        AddMapChecks(8, Vector2Int.zero);
        Vector2Int last = MapChecks[^1];

        // Ways to Locations|Bosses
        AddMapChecks(4, last);
        boss = MapChecks[^1];
        AddMapChecks(5, last);
        toLoc1 = MapChecks[^1];
        AddMapChecks(5, last);
        toLoc2 = MapChecks[^1];
        AddMapChecks(5, last);
        toLoc3 = MapChecks[^1];


        Gen(MapChecks);
        Print(MapChecks);
    }




    // == CHECKS ==
    void AddMapChecks(int maxN, Vector2Int currentMapCheckPos, int n = 0)  {
        // Additional
        int[] offsets = { -1, 1 };

        // Delta
        Vector2Int currentDelta = Vector2Int.zero;

        // Calculate Offset
        if (UnityEngine.Random.value < 0.5f) {
            currentDelta.x = offsets[UnityEngine.Random.Range(0, 2)];
            if (MapChecks.Contains(currentMapCheckPos + currentDelta)) { currentDelta.x *= -1; }
        }
        else {
            currentDelta.y = offsets[UnityEngine.Random.Range(0, 2)];
            if (MapChecks.Contains(currentMapCheckPos + currentDelta)) { currentDelta.y *= -1; }
        }

        // Append | Continue
        MapChecks.Add(currentMapCheckPos + currentDelta);
        if (n <= maxN) {
            AddMapChecks(maxN, currentMapCheckPos + currentDelta, n+1);
        }
    }




















    public static void Print(List<Vector2Int> points)
    {
        if (points.Count == 0) return;

        int minX = int.MaxValue, minY = int.MaxValue;
        int maxX = int.MinValue, maxY = int.MinValue;

        foreach (var p in points)
        {
            minX = Mathf.Min(minX, p.x);
            minY = Mathf.Min(minY, p.y);
            maxX = Mathf.Max(maxX, p.x);
            maxY = Mathf.Max(maxY, p.y);
        }

        StringBuilder sb = new StringBuilder();

        for (int y = maxY+1; y >= minY-1; y--)
        {
            for (int x = minX-1; x <= maxX+1; x++)
            { 
                sb.Append(points.Contains(new Vector2Int(x, y)) ? "__" : "@");
            }
            sb.AppendLine();
        }

        Debug.Log(sb.ToString());
    }















    public void Gen(List<Vector2Int> points)
    {
        if (points.Count == 0 || Prefab == null)
            return;

        Vector2Int start = points[0];
        Vector2Int end = points[^1];

        HashSet<Vector2Int> unique = new HashSet<Vector2Int>(points);

        foreach (var p in unique)
        {
            Vector3 worldPos = new Vector3(
                p.x * cellSize,
                0f,
                p.y * cellSize
            );

            GameObject cube = Instantiate(Prefab, worldPos, Quaternion.identity, transform);

            Renderer r = cube.GetComponent<Renderer>();
            if (r != null)
            {
                if (p == Vector2Int.zero) r.material.color = Color.yellow;
                else if(p == boss) r.material.color = Color.red;
                else if (p == toLoc1) r.material.color = Color.green;
                else if (p == toLoc2) r.material.color = Color.blue;
                else if (p == toLoc3) r.material.color = Color.purple;
                else { r.material.color = Color.white; }
            }
        }

    }




    
}




