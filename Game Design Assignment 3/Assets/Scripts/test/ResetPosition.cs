using UnityEngine;
using System.Collections;
/// <summary>
/// this method use to reset gameObject position when OnCollisionEnter( target )
/// </summary>
public class ResetPosition : MonoBehaviour {
	//private Vector3 beginPosition;
	[SerializeField]
	private string target;

	// Use this for initialization
	void Start () {
		//beginPosition = gameObject.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter(Collision collisionInfo) {
		/*
		if (collisionInfo.gameObject.name.Equals (target)) {

			//t.y = 100;
			gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, beginPosition, 0);
			//GameObject.Find("WeaponCube 1").GetComponent<Rigidbody>().useGravity = false;
			gameObject.GetComponent<Rigidbody> ().useGravity = false;
		}
		*/
		if (collisionInfo.gameObject.name.Equals (target)) {
			GameObject.Destroy(gameObject);
		}
	}
}
