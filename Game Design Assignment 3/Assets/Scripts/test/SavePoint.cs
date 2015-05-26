using UnityEngine;
using System.Collections;

public class SavePoint : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter(Collision collisionInfo) {
		if (collisionInfo.gameObject.name.Equals("Player")) {
			GameObject go = GameObject.Find("stage 1");
			foreach(Transform child in go.transform){
				GameObject.Destroy(child.gameObject);
			}
		}
	}
}
