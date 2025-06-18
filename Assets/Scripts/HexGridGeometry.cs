using UnityEngine;

public class HexGridGeometry
{
    private float _hexRadius;
    private Vector3 _origin;

    public HexGridGeometry(float hexRadius, Vector3 origin)
    {
        _hexRadius = hexRadius;
        _origin = origin;
    }

    public Vector3 HexToWorld(Vector2Int hex)
    {
        float x = _hexRadius * Mathf.Sqrt(3f) * (hex.x + 0.5f * (hex.y % 2));
        float z = _hexRadius * 1.5f * hex.y;
        return _origin + new Vector3(x, 0f, z);
    }

    public Vector2Int WorldToHex(Vector3 world)
    {
        Vector3 local = world - _origin;

        float q = (local.x * Mathf.Sqrt(3f) / 3f - local.z / 3f) / _hexRadius;
        float r = local.z * 2f / 3f / _hexRadius;

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
}
