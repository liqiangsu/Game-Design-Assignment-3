using UnityEngine;
using System.Collections;

public static class Extesions
{

    public static bool Equals2D(this Vector3 v3, Vector3 other)
    {
        var v1 = new Vector3(v3.x,0, v3.z).magnitude;
        var v2 = new Vector3(other.x,0, other.z).magnitude;
        return Mathf.Abs(v1 - v2) < 0.1;
    }

    public static Vector3 BlackHole = new Vector3(99, 99, 99);

}
