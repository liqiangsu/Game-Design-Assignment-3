using System;
using UnityEngine;
using System.Collections;

public static class Extesions
{

    private static float positionPresision = 0.05f;
    public static bool Equals2D(this Vector3 v3, Vector3 other)
    {
        //var v1 = new Vector2(v3.x, v3.z);
        //var v2 = new Vector2(other.x, other.z);
        return Mathf.Abs(v3.x - other.x) < positionPresision && Mathf.Abs(v3.z - other.z )< positionPresision;
        //return v1 == v2;
    }

    public static Vector3 BlackHole = new Vector3(99, 99, 99);

}
