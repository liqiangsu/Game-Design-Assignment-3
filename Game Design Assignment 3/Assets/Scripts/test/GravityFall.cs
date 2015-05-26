using UnityEngine;
using System.Collections;

public class GravityFall : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionExit(Collision collisionInfo) {
		if(collisionInfo.gameObject.name.Equals("Player")){
			Debug.Log(gameObject.GetComponent<MoveTwoWay>().enabled + "  start");
			gameObject.GetComponent<MoveTwoWay>().enabled = false;
			Debug.Log(gameObject.GetComponent<MoveTwoWay>().enabled + "  after");
			gameObject.GetComponent<Rigidbody>().useGravity = true;
		}
	}
}
