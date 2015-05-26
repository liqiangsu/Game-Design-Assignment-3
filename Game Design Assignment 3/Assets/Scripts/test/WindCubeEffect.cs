using UnityEngine;
using System.Collections;

public class WindCubeEffect : MonoBehaviour {
	[SerializeField]
	private Vector3 v3;

	public float speed;
	//public float direction;
	//[SerializeField]
	//private string targetTag;
	//[SerializeField]
	//private string targetTag2;



	// Use this for initialization
	void Start () {
		/*
		if (direction == 1) {
			v3 = Vector3.left;
		} else if (direction == 2) {
			v3 = Vector3.right;
		} else if (direction == 3) {
			v3 = Vector3.forward;
		} else {
			v3 = Vector3.back;
		}
		*/

	}
	
	// Update is called once per frame
	void Update () {}

	void OnCollisionStay(Collision collisionInfo) {

		GameObject go = collisionInfo.gameObject;
		//if (go.CompareTag (targetTag) || go.CompareTag(targetTag2)) {
			go.transform.Translate (v3 * Time.deltaTime * speed,Space.World);
		//}


	}
}
