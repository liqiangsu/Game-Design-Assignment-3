using UnityEngine;
using System.Collections;

public class CollisionTrap : MonoBehaviour {
	private GameObject player;
	private bool stayCollision = false;
	// Use this for initialization
	void Start () {
		player = GameObject.Find ("Player");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter(Collision collisionInfo) {
		if (stayCollision) {
			GameObject go = collisionInfo.gameObject;
			if(go.CompareTag("TrapWall")) 
			{
				GameObject.Destroy(player);
			}
		}
	}
	void OnCollisionExit(Collision collisionInfo) {
		GameObject go = collisionInfo.gameObject;
		if(go.CompareTag("TrapWall"))
		{
			stayCollision = false;
		}
	}
	void OnCollisionStay(Collision collisionInfo) {
		GameObject go = collisionInfo.gameObject;
		if(go.CompareTag("TrapWall"))
		{
			stayCollision = true;
		}
	}
}
