using UnityEngine;
using System.Collections;

public static class Extesions {

	public static bool Equals2D(this Vector3 v3, Vector3 other )
	{
	    var v1 = new Vector2(v3.x, v3.z);
	    var v2 = new Vector2(other.x, other.z);
	    return v1 == v2;
	}
}
