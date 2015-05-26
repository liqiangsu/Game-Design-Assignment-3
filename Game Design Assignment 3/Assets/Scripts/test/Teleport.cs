using UnityEngine;
using System.Collections;

public class Teleport : MonoBehaviour {
	[SerializeField]
	private string TeleportName;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter(Collision collisionInfo) {

		if ("" == TeleportName) {
			//Debug.Log("object destroy");

			GameObject.Destroy (gameObject);
		} else {
			if (collisionInfo.gameObject.CompareTag ("Player")) {
				Vector3 target = GameObject.Find (TeleportName).transform.position;
				target.y += 2;
				//Debug.Log(target.x + "  " + target.y + "  " + target.z);
				//GameObject.Destroy(GameObject.Find("Teleport 1"));
				//GameObject.Destroy(GameObject.Find(TeleportName));
				//GameObject.Find("Player").transform.Translate(target);
				GameObject.Find ("Player").transform.position = Vector3.Lerp (GameObject.Find ("Player").transform.position, target, 1);

			}
		}
	}
}
