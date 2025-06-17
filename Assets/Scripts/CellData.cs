using UnityEngine;

public class CellData
{
    public Vector2Int Hex { get; private set; }
    public bool IsOccupied { get; set; }
    public Slime Slime { get; set; }

    public CellData(Vector2Int hex)
    {
        Hex = hex;
        IsOccupied = false;
        Slime = null;
    }
}
