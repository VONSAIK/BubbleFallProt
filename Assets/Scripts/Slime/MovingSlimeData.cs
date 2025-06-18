using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingSlimeData 
{
    public Slime Slime;
    public Vector3 Direction;
    public LayerMask StopLayers;
    public bool IsFrozen;

    public MovingSlimeData(Slime slime, Vector3 direction)
    {
        Slime = slime;
        Direction = direction.normalized;
        StopLayers = LayerMask.GetMask("Slime", "Wall");
        IsFrozen = false;
    }
}
