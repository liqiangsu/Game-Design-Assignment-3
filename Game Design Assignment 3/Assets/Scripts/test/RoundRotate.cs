using UnityEngine;
using System.Collections;

public class RoundRotate : MonoBehaviour {
	//private GameObject player;
	[SerializeField]
	Vector3 angles;
	//[SerializeField]
	//float Delay;
	//bool PlayerRotate;

	IEnumerator Start()
	{
		//GameObject player = GameObject.Find ("GroundCube 35");
		//BeginPosition = transform.position;
		//EndPositiosn = transform.FindChild("MovingTarget").position;
		while (true)
		{
			yield return StartCoroutine(RotateObject(transform, angles));
			//if((transform.rotation.x >= 179.5 && transform.rotation.x <= 180.5) || (transform.rotation.x >= 0 && transform.rotation.x <= 1)){

			//Debug.Log(transform.rotation.x);
			//if(transform.rotation.eulerAngles.x <= 360  && transform.rotation.x >= 358){
			//	yield return new WaitForSeconds(Delay);
			//}
		}
	}

	void Update() 
	{
		//gameObject.transform.Rotate (angles);
	}

	IEnumerator RotateObject(Transform tf, Vector3 rotateAngles) {
		tf.Rotate (rotateAngles);
		//if (PlayerRotate) {
			//player.transform.Rotate(rotateAngles);
		//}

		yield return null;
	}
	/*
	void OnCollisionStay(Collision collisionInfo) {

		if (collisionInfo.transform.name.Equals ("Player")) {

			PlayerRotate = true;
			player.GetComponent<Rigidbody>().useGravity = false;
		}
	}

	void OnCollisionExit(Collision collisionInfo) {
		if (collisionInfo.transform.name.Equals ("Player")) {

			PlayerRotate = false;
			player.GetComponent<Rigidbody>().useGravity = true;
		}
	}
	*/
}
