using UnityEngine;
using System.Collections;

public class TrapEffect : MonoBehaviour {

	void OnCollisionEnter(Collision collisionInfo) {
		GameObject go = collisionInfo.gameObject;
		if (go.CompareTag("Player")) {
			GameObject.Destroy (go);
		}
	}
}
