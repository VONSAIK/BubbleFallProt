using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class HexGridMatrix
{
    private Dictionary<Vector2Int, Slime> _grid = new();
    private HexGridGeometry _geometry;

    public HexGridMatrix(HexGridGeometry geometry)
    {
        _geometry = geometry;
    }

    public static readonly Vector2Int[] HexDirections =
    {
        new Vector2Int(1, 0), new Vector2Int(0, 1), new Vector2Int(-1, 1),
        new Vector2Int(-1, 0), new Vector2Int(0, -1), new Vector2Int(1, -1)
    };

    public void InitializeGrid(int columns, int rows)
    {
        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                var hex = new Vector2Int(x, y);
                _grid[hex] = null;
            }
        }
    }

    public bool IsOccupied(Vector2Int hex) => _grid.ContainsKey(hex) && _grid[hex] != null;

    public Slime GetSlime(Vector2Int hex) => _grid.TryGetValue(hex, out var slime) ? slime : null;

    public void Register(Slime slime, Vector2Int hex)
    {
        if (!_grid.ContainsKey(hex))
            _grid[hex] = null;

        _grid[hex] = slime;
        slime.transform.position = _geometry.HexToWorld(hex);
    }

    public void Unregister(Vector2Int hex)
    {
        if (_grid.ContainsKey(hex))
            _grid[hex] = null;
    }

    public Vector2Int GetHexFromWorld(Vector3 position)
    {
        return _geometry.WorldToHex(position);
    }

    public Vector3 GetWorldFromHex(Vector2Int hex)
    {
        return _geometry.HexToWorld(hex);
    }

    public Vector2Int? FindClosestNeighbour(Vector2Int center, Vector3 hitPosition)
    {
        Vector2Int? closest = null;
        float minDistance = float.MaxValue;

        foreach (var dir in HexDirections)
        {
            var neighbor = center + dir;
            if (_grid.ContainsKey(neighbor) && _grid[neighbor] != null)
                continue;

            Vector3 worldPos = _geometry.HexToWorld(neighbor);
            float dist = Vector3.Distance(worldPos, hitPosition);
            if (dist < minDistance)
            {
                minDistance = dist;
                closest = neighbor;
            }
        }

        return closest;
    }

    public IEnumerable<Vector2Int> GetNeighbours(Vector2Int hex)
    {
        foreach (var dir in HexDirections)
            yield return hex + dir;
    }

    public Dictionary<Vector2Int, Slime> GetAllCells() => _grid;

    public HashSet<Vector2Int> GetConnectedGroup(Vector2Int start, SlimeColor color)
    {
        var result = new HashSet<Vector2Int>();
        var visited = new HashSet<Vector2Int>();
        var toCheck = new Queue<Vector2Int>();

        toCheck.Enqueue(start);

        while (toCheck.Count > 0)
        {
            var current = toCheck.Dequeue();
            if (visited.Contains(current)) continue;
            visited.Add(current);

            var slime = GetSlime(current);
            if (slime == null || slime.SlimeColor != color) continue;

            result.Add(current);

            foreach (var neighbor in GetNeighbours(current))
            {
                if (!visited.Contains(neighbor))
                    toCheck.Enqueue(neighbor);
            }
        }

        return result;
    }

    public Vector2Int? GetHexOfSlime(Slime slime)
    {
        foreach (var pair in _grid)
            if (pair.Value == slime)
                return pair.Key;

        return null;
    }


    public void RemoveGroup(HashSet<Vector2Int> group)
    {
        foreach (var hex in group)
            Unregister(hex);
    }

    public void DebugPrintGrid(int maxRows = 50, int maxCols = 12)
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("=== HEX GRID STATE ===");

        for (int row = 0; row < maxRows; row++)
        {
            sb.Append(row % 2 == 1 ? " " : "");
            for (int col = 0; col < maxCols; col++)
            {
                Vector2Int hex = new Vector2Int(col, row);

                if (_grid.TryGetValue(hex, out Slime slime) && slime != null)
                {
                    char colorChar = GetColorChar(slime.SlimeColor);
                    sb.Append($"[{colorChar}]");
                }
                else
                {
                    sb.Append("[ ]");
                }
            }
            sb.AppendLine();
        }

        sb.AppendLine("=======================");
        Debug.Log(sb.ToString());
    }

    private char GetColorChar(SlimeColor color)
    {
        return color switch
        {
            SlimeColor.Red => 'R',
            SlimeColor.Blue => 'B',
            SlimeColor.Yellow => 'Y',
            _ => '?'
        };
    }
}
