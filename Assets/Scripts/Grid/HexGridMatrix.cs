using System.Collections.Generic;
using System.Linq;
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

    public static readonly Vector2Int[] EvenRowDirections =
    {
        new Vector2Int(1, 0),   
        new Vector2Int(0, 1),   
        new Vector2Int(-1, 1),  
        new Vector2Int(-1, 0),  
        new Vector2Int(-1, -1), 
        new Vector2Int(0, -1),  
    };

    public static readonly Vector2Int[] OddRowDirections =
    {
        new Vector2Int(1, 0),   
        new Vector2Int(1, 1),   
        new Vector2Int(0, 1),   
        new Vector2Int(-1, 0),  
        new Vector2Int(0, -1),  
        new Vector2Int(1, -1),  
    };

    public IEnumerable<Vector2Int> GetNeighbours(Vector2Int hex)
    {
        var directions = hex.y % 2 == 0 ? EvenRowDirections : OddRowDirections;

        foreach (var dir in directions)
        {
            var neighbor = hex + dir;
            if (_grid.ContainsKey(neighbor))
                yield return neighbor;
        }
    }


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
        {
            Debug.LogWarning($"Hex {hex} поза межами сітки! Реєстрація не відбулася.");
            return;
        }

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

    public Dictionary<Vector2Int, Slime> GetAllCells() => _grid;
    public bool IsValidHex(Vector2Int hex)
    {
        return _grid.ContainsKey(hex);
    }


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

            if (!_grid.ContainsKey(current)) continue;

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

    public HashSet<Vector2Int> GetFloatingSlimes()
    {
        var connectedToTop = new HashSet<Vector2Int>();
        var toCheck = new Queue<Vector2Int>();

        int topRowY = _grid.Keys.Max(k => k.y); 

        foreach (var kvp in _grid)
        {
            Vector2Int hex = kvp.Key;
            Slime slime = kvp.Value;

            if (hex.y == topRowY && slime != null)
            {
                connectedToTop.Add(hex);
                toCheck.Enqueue(hex);
            }
        }

        while (toCheck.Count > 0)
        {
            var current = toCheck.Dequeue();

            foreach (var neighbor in GetNeighbours(current))
            {
                if (connectedToTop.Contains(neighbor)) continue;

                if (_grid.TryGetValue(neighbor, out var slime) && slime != null)
                {
                    connectedToTop.Add(neighbor);
                    toCheck.Enqueue(neighbor);
                }
            }
        }

        var floating = new HashSet<Vector2Int>();

        foreach (var kvp in _grid)
        {
            if (kvp.Value != null && !connectedToTop.Contains(kvp.Key))
            {
                floating.Add(kvp.Key);
            }
        }

        return floating;
    }

    public Vector2Int? GetHexOfSlime(Slime slime)
    {
        foreach (var pair in _grid)
            if (pair.Value == slime)
                return pair.Key;

        return null;
    }
    
    public void DebugPrintGrid(int maxRows = 50, int maxCols = 11)
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
