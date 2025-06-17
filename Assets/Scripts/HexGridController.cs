using System.Collections.Generic;
using UnityEngine;

public class HexGridController : MonoBehaviour, IService
{
    [SerializeField] private int rows = 7;
    [SerializeField] private int columns = 11;
    [SerializeField] private float hexRadius = 0.32f;
    [SerializeField] private Transform origin;

    private float hexWidth;
    private float hexHeight;

    private Dictionary<Vector2Int, CellData> grid = new();

    public void Init()
    {
        hexWidth = hexRadius * 2f;
        hexHeight = Mathf.Sqrt(3f) * hexRadius;

        GenerateGrid(rows, columns);
    }

    private void GenerateGrid(int rowCount, int columnCount)
    {
        for (int r = 0; r < rowCount; r++)
        {
            for (int q = 0; q < columnCount; q++)
            {
                int offset = r / 2; 
                Vector2Int hex = new Vector2Int(q - offset, r);

                if (!grid.ContainsKey(hex))
                    grid[hex] = new CellData(hex);
            }
        }
    }

    public Vector3 HexToWorld(Vector2Int hex)
    {
        float x = hexRadius * Mathf.Sqrt(3f) * (hex.x + hex.y / 2f);
        float z = hexRadius * 3f / 2f * hex.y;

        return origin.position + new Vector3(x, 0f, z);
    }

    public List<Vector2Int> GetEmptyCells()
    {
        List<Vector2Int> empty = new();

        foreach (var kv in grid)
        {
            if (!kv.Value.IsOccupied)
                empty.Add(kv.Key);
        }

        return empty;
    }


    public void RegisterSlime(Slime slime, Vector2Int hex)
    {
        if (!grid.ContainsKey(hex))
        {
            Debug.LogError($"Hex {hex} does not exist in the grid!");
            return;
        }

        var cell = grid[hex];

        if (cell.IsOccupied)
        {
            Debug.LogWarning($"Cell {hex} is already occupied!");
            return;
        }

        // Оновлюємо дані клітинки
        cell.Slime = slime;
        cell.IsOccupied = true;

        // Переміщуємо slime в потрібну позицію в світі
        slime.transform.position = HexToWorld(hex);
    }



    private void OnDrawGizmos()
    {
        if (grid == null || grid.Count == 0)
            return;

        Gizmos.color = Color.green;
        foreach (var cell in grid.Values)
        {
            Vector3 pos = HexToWorld(cell.Hex);
            Gizmos.DrawSphere(pos, 0.05f);
        }
    }
}
