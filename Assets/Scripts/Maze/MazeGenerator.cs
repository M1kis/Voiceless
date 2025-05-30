using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    [Header("Tamaño del laberinto (usa valores impares)")]
    public int width = 21;
    public int height = 21;
    public float cellSize = 2f;

    [Header("Prefabs")]
    public GameObject wallPrefab;
    public GameObject entradaPrefab;
    public GameObject salidaPrefab;
    public GameObject pelletPrefab;

    [Header("Player")]
    public Transform player;

    private int[,] maze;
    private Vector2Int entrada;
    private Vector2Int salida;

    void Start()
    {
        GenerateMaze();
        DrawMaze();
        SetEntradaYSalida();
        if (player != null) MovePlayerToEntrada();
    }

    void GenerateMaze()
    {
        maze = new int[width, height];

        // Inicializa todo como muro
        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                maze[x, y] = 0;

        // Inicia desde una celda impar dentro del laberinto
        Vector2Int start = new Vector2Int(1, 1);
        maze[start.x, start.y] = 1;

        RecursiveBacktrack(start);
    }

    void RecursiveBacktrack(Vector2Int current)
    {
        List<Vector2Int> directions = new List<Vector2Int>
        {
            Vector2Int.up * 2,
            Vector2Int.down * 2,
            Vector2Int.left * 2,
            Vector2Int.right * 2
        };

        Shuffle(directions);

        foreach (var dir in directions)
        {
            Vector2Int neighbor = current + dir;
            if (InBounds(neighbor) && maze[neighbor.x, neighbor.y] == 0)
            {
                Vector2Int wall = current + dir / 2;
                maze[wall.x, wall.y] = 1;
                maze[neighbor.x, neighbor.y] = 1;
                RecursiveBacktrack(neighbor);
            }
        }
    }

    void Shuffle(List<Vector2Int> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            Vector2Int temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }
    }

    void DrawMaze()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 cellPos = new Vector3(x * cellSize, 0, y * cellSize);

                if (maze[x, y] == 0)
                {
                    Instantiate(wallPrefab, cellPos, Quaternion.identity, transform);
                }
                else
                {
                    // Rodear con paredes si vecino es muro
                    for (int dx = -1; dx <= 1; dx++)
                    {
                        for (int dy = -1; dy <= 1; dy++)
                        {
                            if (Mathf.Abs(dx) + Mathf.Abs(dy) != 1) continue;
                            int nx = x + dx;
                            int ny = y + dy;

                            if (!InBounds(new Vector2Int(nx, ny)) || maze[nx, ny] == 0)
                            {
                                Vector3 wallPos = cellPos + new Vector3(dx * cellSize / 2f, 0, dy * cellSize / 2f);
                                Quaternion rot = (dx != 0) ? Quaternion.Euler(0, 90, 0) : Quaternion.identity;
                                Instantiate(wallPrefab, wallPos, rot, transform);
                            }
                        }
                    }

                    if (pelletPrefab != null)
                    {
                        Vector3 pelletPos = cellPos + new Vector3(0, 0.25f, 0);
                        Instantiate(pelletPrefab, pelletPos, Quaternion.identity, transform);
                    }
                }
            }
        }
    }

    void SetEntradaYSalida()
    {
        List<Vector2Int> posiblesEntradas = new List<Vector2Int>();
        List<Vector2Int> posiblesSalidas = new List<Vector2Int>();

        for (int i = 1; i < width - 1; i++)
        {
            if (maze[i, 1] == 1) posiblesEntradas.Add(new Vector2Int(i, 0));
            if (maze[i, height - 2] == 1) posiblesSalidas.Add(new Vector2Int(i, height - 1));
        }

        for (int j = 1; j < height - 1; j++)
        {
            if (maze[1, j] == 1) posiblesEntradas.Add(new Vector2Int(0, j));
            if (maze[width - 2, j] == 1) posiblesSalidas.Add(new Vector2Int(width - 1, j));
        }

        if (posiblesEntradas.Count == 0 || posiblesSalidas.Count == 0)
        {
            Debug.LogWarning("No se encontraron entradas o salidas válidas.");
            return;
        }

        entrada = posiblesEntradas[Random.Range(0, posiblesEntradas.Count)];
        salida = posiblesSalidas[Random.Range(0, posiblesSalidas.Count)];

        if (entradaPrefab != null)
        {
            Vector3 entradaPos = new Vector3(entrada.x * cellSize, 0.5f, entrada.y * cellSize);
            Instantiate(entradaPrefab, entradaPos, Quaternion.identity, transform);
        }

        if (salidaPrefab != null)
        {
            Vector3 salidaPos = new Vector3(salida.x * cellSize, 0.5f, salida.y * cellSize);
            Instantiate(salidaPrefab, salidaPos, Quaternion.identity, transform);
        }
    }

    void MovePlayerToEntrada()
    {
        Vector3 playerPos = new Vector3(entrada.x * cellSize, player.position.y, entrada.y * cellSize);
        player.position = playerPos;
    }

    bool InBounds(Vector2Int pos)
    {
        return pos.x > 0 && pos.x < width - 1 && pos.y > 0 && pos.y < height - 1;
    }
}
