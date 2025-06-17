using UnityEngine;
using System.Collections.Generic;

public class SlimeHexGridSpawner : MonoBehaviour, IService
{
    private PoolsController _pools;
    private HexGridController _grid;

    public void Init()
    {
        _pools = ServiceLocator.Current.Get<PoolsController>();
        _grid = ServiceLocator.Current.Get<HexGridController>();

        SpawnSlimesOnGrid();
    }

    private void SpawnSlimesOnGrid()
    {
        List<Vector2Int> emptyCells = _grid.GetEmptyCells();

        foreach (var hex in emptyCells)
        {
            Slime slime = _pools.GetSlime();
            _grid.RegisterSlime(slime, hex);
        }
    }
}
