using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

public class Ectorris_MapController : MonoBehaviour
{
    // == Checks ==
    public List<Vector2Int> MapChecks = new List<Vector2Int>() { };

    // == Rooms ==
    public List<GameObject> FourExitRooms = new List<GameObject>();

    public List<GameObject> ThreeExits = new List<GameObject>();
    public List<GameObject> TwoExitsTogether = new List<GameObject>();
    public List<GameObject> TwoExitsOnOppositeSides = new List<GameObject>();
    public List<GameObject> OneExit = new List<GameObject>();

    public GameObject Prefab;
    public float cellSize = 10f;

    // ==== START ====
    private void Start()
    {
        // == Checks ==
        // Start-way
        AddMapChecks(10, Vector2Int.zero);
        Vector2Int last = MapChecks[^1];

        // Ways to Locations|Bosses
        AddMapChecks(7, last);
        AddMapChecks(7, last);
        AddMapChecks(7, last);
        AddMapChecks(7, last);

        // == Rooms ==
        SeparateRooms();
        print(MapChecks);
    }

    // == CHECKS ==
    private void AddMapChecks(int maxN, Vector2Int currentMapCheckPos, int n = 0)
    {
        // Additional
        int[] offsets = { -1, 1 };

        // Delta
        Vector2Int currentDelta = Vector2Int.zero;

        // Calculate Offset
        if (UnityEngine.Random.value < 0.5f)
        {
            currentDelta.x = offsets[UnityEngine.Random.Range(0, 2)];
            if (MapChecks.Contains(currentMapCheckPos + currentDelta)) { currentDelta.x *= -1; }
        }
        else
        {
            currentDelta.y = offsets[UnityEngine.Random.Range(0, 2)];
            if (MapChecks.Contains(currentMapCheckPos + currentDelta)) { currentDelta.y *= -1; }
        }

        // Append | Continue
        MapChecks.Add(currentMapCheckPos + currentDelta);
        if (n <= maxN)
        {
            AddMapChecks(maxN, currentMapCheckPos + currentDelta, n + 1);
        }
    }

    // == ROOMS ==
    private void SeparateRooms()
    {
        // Additional
        List<string> Lines = new List<string>();

        // Min|Max
        int minX = int.MaxValue, minY = int.MaxValue;
        int maxX = int.MinValue, maxY = int.MinValue;

        foreach (var p in MapChecks)
        {
            minX = Mathf.Min(minX, p.x);
            minY = Mathf.Min(minY, p.y);
            maxX = Mathf.Max(maxX, p.x);
            maxY = Mathf.Max(maxY, p.y);
        }

        Vector2Int StartPoint = Vector2Int.zero;
        StartPoint = new Vector2Int(-minX, -minY);

        int i = 0;
        for (int Line = maxY + 1; Line >= minY - 1; Line--)
        {
            Lines.Add("");
            for (int x = minX - 1; x <= maxX + 1; x++)
            {
                Lines[i] += MapChecks.Contains(new Vector2Int(x, Line)) ? "_" : "#";
            }
            i += 1;
        }

        for (int lineN = 0; lineN < Lines.Count; lineN++)
        {
            string line = Lines[lineN];
            for (int x = 0; x < line.Length; x++)
            {
                if (line[x] == '_')
                {
                    bool upIsWall = Lines[lineN + 1][x] == '#';
                    bool downIsWall = Lines[lineN - 1][x] == '#';
                    bool rightIsWall = Lines[lineN][x + 1] == '#';
                    bool leftIsWall = Lines[lineN][x - 1] == '#';

                    int count = 0;
                    if (upIsWall) count++;
                    if (downIsWall) count++;
                    if (rightIsWall) count++;
                    if (leftIsWall) count++;

                    Vector2Int p = new Vector2Int(lineN, x);
                    Vector3 worldPos = new Vector3(
                        lineN * cellSize,
                        0f,
                        x * cellSize
                    );

                    Renderer r = null;

                    if (count == 0)
                    {
                        GameObject room = Instantiate(FourExitRooms[0], worldPos, Quaternion.identity, transform);
                        r = room.transform.GetChild(0).GetComponent<Renderer>();
                    }
                    if (count == 1)
                    {
                        GameObject room = Instantiate(ThreeExits[0], worldPos, Quaternion.identity, transform);
                        r = room.transform.GetChild(0).GetComponent<Renderer>();
                        if (upIsWall) room.transform.Rotate(new Vector3(0, 90, 0));
                        if (leftIsWall) room.transform.Rotate(new Vector3(0, 180, 0));
                        if (downIsWall) room.transform.Rotate(new Vector3(0, 270, 0));
                    }
                    if (count == 2)
                    {
                        if (upIsWall && downIsWall)
                        {
                            GameObject room = Instantiate(TwoExitsOnOppositeSides[0], worldPos, Quaternion.identity, transform);
                            r = room.transform.GetChild(0).GetComponent<Renderer>();
                        }
                        if (leftIsWall && rightIsWall)
                        {
                            GameObject room = Instantiate(TwoExitsOnOppositeSides[0], worldPos, Quaternion.identity, transform);
                            r = room.transform.GetChild(0).GetComponent<Renderer>();
                            room.transform.Rotate(new Vector3(0, 90, 0));
                        }

                        if (leftIsWall)
                        {
                            if (upIsWall)
                            {
                                GameObject room = Instantiate(TwoExitsTogether[0], worldPos, Quaternion.identity, transform);
                                r = room.transform.GetChild(0).GetComponent<Renderer>();
                                room.transform.Rotate(new Vector3(0, 180, 0));
                            }
                            if (downIsWall)
                            {
                                GameObject room = Instantiate(TwoExitsTogether[0], worldPos, Quaternion.identity, transform);
                                r = room.transform.GetChild(0).GetComponent<Renderer>();
                                room.transform.Rotate(new Vector3(0, -90, 0));
                            }
                        }
                        if (rightIsWall)
                        {
                            if (upIsWall)
                            {
                                GameObject room = Instantiate(TwoExitsTogether[0], worldPos, Quaternion.identity, transform);
                                r = room.transform.GetChild(0).GetComponent<Renderer>();
                                room.transform.Rotate(new Vector3(0, 90, 0));
                            }
                            if (downIsWall)
                            {
                                GameObject room = Instantiate(TwoExitsTogether[0], worldPos, Quaternion.identity, transform);
                                r = room.transform.GetChild(0).GetComponent<Renderer>();
                            }
                        }
                    }
                    if (count == 3)
                    {
                        GameObject room = Instantiate(OneExit[0], worldPos, Quaternion.identity, transform);
                        r = room.transform.GetChild(0).GetComponent<Renderer>();
                        if (downIsWall) room.transform.Rotate(new Vector3(0, 90, 0));
                        if (leftIsWall) room.transform.Rotate(new Vector3(0, 180, 0));
                        if (upIsWall) room.transform.Rotate(new Vector3(0, 270, 0));
                    }

                    print(p);
                    if (p == StartPoint) r.material.color = Color.white;
                }
            }
        }
    }
}