using System.Collections.Generic;
using UnityEngine;
using CustomEventBus;
using CustomEventBus.Signals;
using System.Linq;

public class HexGridController : MonoBehaviour, IService
{
    private static readonly Vector2Int[] HexNeighbours = new Vector2Int[]
    {
        new Vector2Int(1, 0), new Vector2Int(0, 1), new Vector2Int(-1, 1),
        new Vector2Int(-1, 0), new Vector2Int(0, -1), new Vector2Int(1, -1)
    };

    [SerializeField] private int rows = 8;
    [SerializeField] private int columns = 11;
    [SerializeField] private float hexRadius = 0.32f;
    [SerializeField] private Transform origin;

    private float hexWidth;
    private float hexHeight;

    private EventBus _eventBus;

    private Dictionary<Vector2Int, CellData> _grid = new();

    public void Init()
    {
        hexWidth = hexRadius * 2f;
        hexHeight = Mathf.Sqrt(3f) * hexRadius;

        GenerateGrid();
        _eventBus = ServiceLocator.Current.Get<EventBus>();
        _eventBus.Subscride<SlimeLandedSignal>(OnSlimeLanded);
    }

    private void GenerateGrid()
    {
        _grid.Clear();

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                Vector2Int hex = new Vector2Int(col, row);
                if (!_grid.ContainsKey(hex))
                    _grid.Add(hex, new CellData(hex));
            }
        }
    }

    public Vector3 HexToWorld(Vector2Int hex)
    {
        float x = hexRadius * Mathf.Sqrt(3f) * (hex.x + 0.5f * (hex.y % 2));
        float z = hexRadius * 1.5f * hex.y;
        return origin.position + new Vector3(x, 0f, z);
    }

    public Vector2Int WorldToHex(Vector3 worldPosition)
    {
        Vector3 local = worldPosition - origin.position;

        float q = (local.x * Mathf.Sqrt(3f) / 3f - local.z / 3f) / hexRadius;
        float r = local.z * 2f / 3f / hexRadius;

        return CubeRound(q, r);
    }

    private Vector2Int CubeRound(float q, float r)
    {
        float x = q;
        float z = r;
        float y = -x - z;

        int rx = Mathf.RoundToInt(x);
        int ry = Mathf.RoundToInt(y);
        int rz = Mathf.RoundToInt(z);

        float dx = Mathf.Abs(rx - x);
        float dy = Mathf.Abs(ry - y);
        float dz = Mathf.Abs(rz - z);

        if (dx > dy && dx > dz) rx = -ry - rz;
        else if (dy > dz) ry = -rx - rz;
        else rz = -rx - ry;

        return new Vector2Int(rx, rz);
    }

    public void RegisterSlime(Slime slime, Vector2Int hex)
    {
        if (!_grid.ContainsKey(hex))
            _grid.Add(hex, new CellData(hex));

        _grid[hex].Slime = slime;
        slime.transform.position = HexToWorld(hex);
    }

    private Vector2Int GetHexOfSlime(Slime slime)
    {
        return WorldToHex(slime.transform.position);
    }

    private void OnSlimeLanded(SlimeLandedSignal signal)
    {
        Vector3 landedPos = signal.Slime.transform.position;

        Slime closestSlime = null;
        float minDist = float.MaxValue;
        Vector2Int closestHex = Vector2Int.zero;

        foreach (var kvp in _grid)
        {
            if (kvp.Value.IsOccupied && kvp.Value.Slime != signal.Slime)
            {
                Vector3 slimeWorldPos = HexToWorld(kvp.Key);
                float dist = Vector3.Distance(landedPos, slimeWorldPos);

                if (dist < minDist)
                {
                    minDist = dist;
                    closestSlime = kvp.Value.Slime;
                    closestHex = kvp.Key;
                }
            }
        }

        if (closestSlime == null)
        {
            Debug.LogWarning($"No target slime found near pos: {landedPos}");
            return;
        }

        Vector3 incomingDir = (signal.Slime.transform.position - HexToWorld(closestHex)).normalized;

        Vector2Int? bestHex = null;
        float bestDot = float.NegativeInfinity;

        foreach (var offset in HexNeighbours)
        {
            Vector2Int neighborHex = closestHex + offset;

            if (_grid.ContainsKey(neighborHex) && _grid[neighborHex].IsOccupied)
                continue;

            if (!_grid.ContainsKey(neighborHex))
                _grid[neighborHex] = new CellData(neighborHex);

            Vector3 neighborWorldPos = HexToWorld(neighborHex);
            Vector3 dirToNeighbor = (neighborWorldPos - HexToWorld(closestHex)).normalized;

            float dot = Vector3.Dot(incomingDir, dirToNeighbor);

            if (dot > bestDot)
            {
                bestDot = dot;
                bestHex = neighborHex;
            }
        }

        if (bestHex.HasValue)
        {
            RegisterSlime(signal.Slime, bestHex.Value);
            ServiceLocator.Current.Get<EventBus>().Invoke(new SlimeAttachedSignal(signal.Slime));
        }
        else
        {
            Debug.LogWarning($"No free neighbor found to attach slime near {closestHex}. Slime pos: {landedPos}");
        }
    }

    public List<Vector2Int> GetEmptyCells()
    {
        List<Vector2Int> result = new();
        foreach (var cell in _grid.Values)
        {
            if (!cell.IsOccupied)
                result.Add(cell.Hex);
        }

        return result;
    }

    private void OnDrawGizmos()
    {
        if (_grid == null) return;

        Gizmos.color = Color.green;
        foreach (var cell in _grid.Values)
        {
            Vector3 pos = HexToWorld(cell.Hex);
            Gizmos.DrawSphere(pos, 0.05f);
        }
    }
}
