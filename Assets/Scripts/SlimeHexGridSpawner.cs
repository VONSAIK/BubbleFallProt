using System.Collections.Generic;
using UnityEngine;

public class SlimeHexGridSpawner : MonoBehaviour, IService
{
    [SerializeField] private int rowsToSpawn = 27; 

    private PoolsController _pools;
    private HexGridController _gridController;
    private HexGridMatrix _matrix;

    public void Init()
    {
        _pools = ServiceLocator.Current.Get<PoolsController>();
        _gridController = ServiceLocator.Current.Get<HexGridController>();

        _matrix = _gridController.GetMatrix();

        SpawnSlimesOnGrid();
    }

    private void SpawnSlimesOnGrid()
    {
        int maxRows = Mathf.Min(rowsToSpawn, _gridController.Rows);
        int startRow = _gridController.Rows - maxRows;

        for (int y = _gridController.Rows - 1; y >= startRow; y--)
        {
            for (int x = 0; x < _gridController.Columns; x++)
            {
                Vector2Int hex = new Vector2Int(x, y);
                if (_matrix.IsOccupied(hex)) continue;

                Slime slime = _pools.GetSlime();
                _matrix.Register(slime, hex);
            }
        }
    }

}
