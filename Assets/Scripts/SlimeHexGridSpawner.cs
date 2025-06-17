using UnityEngine;
using System.Collections.Generic;

public class SlimeHexGridSpawner : MonoBehaviour, IService
{
    private PoolsController _poolsController;
    private HexGridController _hexGrid;

    public void Init()
    {
        _poolsController = ServiceLocator.Current.Get<PoolsController>();
        _hexGrid = ServiceLocator.Current.Get<HexGridController>();

        SpawnSlimes();
    }

    private void SpawnSlimes()
    {
        if (_hexGrid == null)
        {
            Debug.LogError("HexGridController not assigned!");
            return;
        }

        List<Vector2Int> emptyCells = _hexGrid.GetEmptyCells();

        foreach (var hex in emptyCells)
        {
            Slime slime = _poolsController.GetSlime();
            _hexGrid.RegisterSlime(slime, hex);
        }
    }
}
