using UnityEngine;

public class CellData
{
    public Vector2Int Hex;
    public Slime Slime;

    public bool IsOccupied => Slime != null;

    public CellData(Vector2Int hex)
    {
        Hex = hex;
        Slime = null;
    }
}
