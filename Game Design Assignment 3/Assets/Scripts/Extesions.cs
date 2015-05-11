using UnityEngine;
using System.Collections;

public static class Extesions {

	public static bool Equals2D(this Vector3 v3, Vector3 other ){
		return v3.x == other.x && v3.z == other.z;
	}
}
