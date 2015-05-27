using UnityEngine;
using System.Collections;

public class BossWeaponTrigger : MonoBehaviour {


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter(Collision collisionInfo) {
		if(collisionInfo.gameObject.name.Equals("Player")) {
			GameObject.Find("WeaponCube 3").GetComponent<Rigidbody>().useGravity = true;
			GameObject.Destroy(gameObject);
		}
	}
}
