using UnityEngine;
using System.Collections;

public class BossTrap : MonoBehaviour {

	private Vector3 startPoint;

	// Use this for initialization
	void Start () {
		startPoint = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionStay(Collision collisionInfo) {


		//Debug.Log (collisionInfo.gameObject.name);
		//GameObject go = collisionInfo.gameObject;
		if(!collisionInfo.transform.tag.Equals("BossTrap")){
			gameObject.GetComponent<Renderer> ().enabled = false;
			gameObject.GetComponent<Rigidbody> ().useGravity = false;
			gameObject.transform.position = startPoint;
		//Vector3 target = tf.position;
		//target.x = tf.position.x;
		//target.y = 25;
		//target.z = tf.position.z;
			
		//tf.position = target;
		}
	}
	void OnCollisionEnter(Collision collisionInfo) {

	}

	void OnCollisionExit(Collision collisionInfo) {

	}
}
