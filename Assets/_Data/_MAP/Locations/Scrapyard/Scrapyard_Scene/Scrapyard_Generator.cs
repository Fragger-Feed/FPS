using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class HighResPerlinPlatform : MonoBehaviour
{
    public int resolution = 100;
    public float size = 10f;
    public float noiseScale = 5f;
    public float height = 2f;
    [Range(0, 1)] public float DownOffset = 0.4f;

    public int roomPadding = 4;

    float[,] heightMap;
    bool[,] roomMap;
    bool[,] keepMask;

    void Start()
    {
        Generate();
    }

    void Generate()
    {
        heightMap = new float[resolution + 1, resolution + 1];
        roomMap = new bool[resolution + 1, resolution + 1];
        keepMask = new bool[resolution + 1, resolution + 1];

        float step = size / resolution;

        int xOffset = Random.Range(-10000, 10000);
        int zOffset = Random.Range(-10000, 10000);

        // ===== 1️⃣ Генерация высот =====
        for (int z = 0; z <= resolution; z++)
        {
            for (int x = 0; x <= resolution; x++)
            {
                float xPos = x * step;
                float zPos = z * step;

                float noise = Mathf.PerlinNoise(
                    xPos / noiseScale + xOffset,
                    zPos / noiseScale + zOffset
                );

                float y = (noise - DownOffset) * height;

                heightMap[x, z] = y;

                if (y <= 0)
                    roomMap[x, z] = true;
            }
        }

        // ===== 2️⃣ Поиск комнат =====
        var rooms = GetRooms();

        // Удаляем те что касаются края
        List<List<Vector2Int>> validRooms = new List<List<Vector2Int>>();

        foreach (var room in rooms)
        {
            if (!IsTouchingEdge(room))
                validRooms.Add(room);
        }

        // ===== 3️⃣ Формируем keepMask только вокруг валидных комнат =====
        foreach (var room in validRooms)
        {
            foreach (var tile in room)
            {
                for (int dz = -roomPadding; dz <= roomPadding; dz++)
                {
                    for (int dx = -roomPadding; dx <= roomPadding; dx++)
                    {
                        int nx = tile.x + dx;
                        int nz = tile.y + dz;

                        if (nx >= 0 && nz >= 0 &&
                            nx <= resolution && nz <= resolution)
                        {
                            keepMask[nx, nz] = true;
                        }
                    }
                }
            }
        }

        // ===== 4️⃣ Строим Mesh =====
        Mesh mesh = new Mesh();
        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;

        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        Dictionary<Vector2Int, int> vertIndex = new Dictionary<Vector2Int, int>();

        for (int z = 0; z <= resolution; z++)
        {
            for (int x = 0; x <= resolution; x++)
            {
                if (!keepMask[x, z])
                    continue;

                if (heightMap[x, z] <= 0)
                    continue; // 🔥 полностью убираем нули

                Vector3 v = new Vector3(x * step, heightMap[x, z], z * step);

                vertIndex[new Vector2Int(x, z)] = vertices.Count;
                vertices.Add(v);
            }
        }

        for (int z = 0; z < resolution; z++)
        {
            for (int x = 0; x < resolution; x++)
            {
                Vector2Int a = new Vector2Int(x, z);
                Vector2Int b = new Vector2Int(x + 1, z);
                Vector2Int c = new Vector2Int(x, z + 1);
                Vector2Int d = new Vector2Int(x + 1, z + 1);

                if (vertIndex.ContainsKey(a) &&
                    vertIndex.ContainsKey(b) &&
                    vertIndex.ContainsKey(c) &&
                    vertIndex.ContainsKey(d))
                {
                    int ia = vertIndex[a];
                    int ib = vertIndex[b];
                    int ic = vertIndex[c];
                    int id = vertIndex[d];

                    triangles.Add(ia);
                    triangles.Add(ic);
                    triangles.Add(ib);

                    triangles.Add(ib);
                    triangles.Add(ic);
                    triangles.Add(id);
                }
            }
        }

        mesh.SetVertices(vertices);
        mesh.SetTriangles(triangles, 0);
        mesh.RecalculateNormals();

        GetComponent<MeshFilter>().mesh = mesh;
    }

    // =================================================

    List<List<Vector2Int>> GetRooms()
    {
        bool[,] visited = new bool[resolution + 1, resolution + 1];
        List<List<Vector2Int>> rooms = new List<List<Vector2Int>>();

        for (int x = 0; x <= resolution; x++)
        {
            for (int z = 0; z <= resolution; z++)
            {
                if (roomMap[x, z] && !visited[x, z])
                    rooms.Add(FloodFill(x, z, visited));
            }
        }

        return rooms;
    }

    List<Vector2Int> FloodFill(int startX, int startZ, bool[,] visited)
    {
        List<Vector2Int> room = new List<Vector2Int>();
        Queue<Vector2Int> queue = new Queue<Vector2Int>();

        queue.Enqueue(new Vector2Int(startX, startZ));
        visited[startX, startZ] = true;

        int[] dx = { 1, -1, 0, 0 };
        int[] dz = { 0, 0, 1, -1 };

        while (queue.Count > 0)
        {
            var tile = queue.Dequeue();
            room.Add(tile);

            for (int i = 0; i < 4; i++)
            {
                int nx = tile.x + dx[i];
                int nz = tile.y + dz[i];

                if (nx >= 0 && nz >= 0 &&
                    nx <= resolution && nz <= resolution)
                {
                    if (roomMap[nx, nz] && !visited[nx, nz])
                    {
                        visited[nx, nz] = true;
                        queue.Enqueue(new Vector2Int(nx, nz));
                    }
                }
            }
        }

        return room;
    }

    bool IsTouchingEdge(List<Vector2Int> room)
    {
        foreach (var tile in room)
        {
            if (tile.x == 0 || tile.y == 0 ||
                tile.x == resolution || tile.y == resolution)
                return true;
        }
        return false;
    }
}