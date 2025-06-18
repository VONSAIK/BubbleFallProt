using System.Collections.Generic;
using UnityEngine;
using CustomEventBus;
using CustomEventBus.Signals;

public class SlimeDropDownController : MonoBehaviour, IService
{
    private HexGridMatrix _matrix;
    private HexGridController _gridController;
    private EventBus _eventBus;

    public void Init()
    {
        _eventBus = ServiceLocator.Current.Get<EventBus>();
        _gridController = ServiceLocator.Current.Get<HexGridController>();
        _matrix = _gridController.GetMatrix();

        _eventBus.Subscride<SlimeStepDownSignal>(OnStepDown);
    }

    private void OnStepDown(SlimeStepDownSignal signal)
    {
        Dictionary<Vector2Int, Slime> currentGrid = _matrix.GetAllCells();
        List<(Vector2Int oldHex, Vector2Int newHex, Slime slime)> moves = new();
        bool gameOver = false;

        foreach (var kvp in currentGrid)
        {
            Vector2Int currentHex = kvp.Key;
            Slime slime = kvp.Value;

            if (slime == null)
                continue;

            Vector2Int targetHex = new Vector2Int(currentHex.x, currentHex.y - 1);

            if (!_matrix.IsValidHex(targetHex))
            {
                gameOver = true;
                continue;
            }

            moves.Add((currentHex, targetHex, slime));
        }

        // Спочатку очистити старі комірки
        foreach (var move in moves)
        {
            _matrix.Unregister(move.oldHex);
        }

        // Потім зареєструвати у нових
        foreach (var move in moves)
        {
            _matrix.Register(move.slime, move.newHex);
        }

        // Оновити верхній ряд (очистити)
        int topRow = _gridController.Rows - 1;
        for (int x = 0; x < _gridController.Columns; x++)
        {
            Vector2Int hex = new Vector2Int(x, topRow);
            if (_matrix.IsValidHex(hex))
                _matrix.Unregister(hex);
        }

        _matrix.DebugPrintGrid();

        if (gameOver)
        {
            Debug.LogWarning("Досягнуто нижньої межі. Гра завершена.");
            //_eventBus.Invoke(new GameOverSignal());
        }
    }
}
