using UnityEngine;
using CustomEventBus;
using CustomEventBus.Signals;

public class HexGridController : MonoBehaviour, IService
{
    [SerializeField] private int rows = 7;
    [SerializeField] private int columns = 12;
    [SerializeField] private float hexRadius = 0.32f;
    [SerializeField] private Transform origin;

    private HexGridGeometry _geometry;
    private HexGridMatrix _matrix;
    private EventBus _eventBus;

    public int Rows => rows;
    public int Columns => columns;




    public void Init()
    {
        _geometry = new HexGridGeometry(hexRadius, origin.position);
        _matrix = new HexGridMatrix(_geometry);

        _matrix.InitializeGrid(columns, rows);

        _eventBus = ServiceLocator.Current.Get<EventBus>();
        _eventBus.Subscribe<SlimeLandedSignal>(OnSlimeLanded);
    }

    public HexGridMatrix GetMatrix() => _matrix;

    private void OnSlimeLanded(SlimeLandedSignal signal)
    {
        Vector3 hitPos = signal.Slime.transform.position;
        Vector2Int landedHex = _matrix.GetHexFromWorld(hitPos);

        Slime closest = null;
        float closestDist = float.MaxValue;
        Vector2Int targetHex = default;

        foreach (var kvp in _matrix.GetAllCells())
        {
            if (kvp.Value == null) continue;

            float dist = Vector3.Distance(_matrix.GetWorldFromHex(kvp.Key), hitPos);
            if (dist < closestDist)
            {
                closestDist = dist;
                closest = kvp.Value;
                targetHex = kvp.Key;
            }
        }

        if (closest == null)
        {
            Debug.LogWarning("Не знайдено сусіднього слайма для прикріплення.");
            return;
        }

        Vector2Int? bestHex = null;
        float minDistance = float.MaxValue;

        foreach (var neighborHex in _matrix.GetNeighbours(targetHex))
        {
            if (_matrix.IsOccupied(neighborHex)) continue;

            Vector3 world = _matrix.GetWorldFromHex(neighborHex);
            float dist = Vector3.Distance(hitPos, world);
            if (dist < minDistance)
            {
                minDistance = dist;
                bestHex = neighborHex;
            }
        }


        if (bestHex.HasValue)
        {
            _matrix.Register(signal.Slime, bestHex.Value);
            _eventBus.Invoke(new SlimeAttachedSignal(signal.Slime));
        }
        else
        {
            Debug.LogWarning($"Не знайдено вільного гекса навколо {targetHex} для прикріплення.");
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (_geometry == null)
            _geometry = new HexGridGeometry(hexRadius, origin.position);

        Gizmos.color = Color.gray;

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                Vector2Int hex = new Vector2Int(col, row);
                Vector3 worldPos = _geometry.HexToWorld(hex);
                Gizmos.DrawWireSphere(worldPos, hexRadius * 0.3f);
            }
        }
    }
}
