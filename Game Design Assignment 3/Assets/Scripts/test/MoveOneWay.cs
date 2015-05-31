using UnityEngine;
using System.Collections;

public class MoveOneWay : MonoBehaviour {
	private bool allowMove = false;

	//private Vector3 beginPosition;
	private GameObject player;


	[SerializeField]
	private Vector3 endPosition;
	[SerializeField]
	private float speed;
	[SerializeField]
	private bool playerMove;
	//[SerializeField]
	//private bool allowReset;
	//[SerializeField]
	//private bool reset;
	// Use this for initialization
	IEnumerator Start () {
		player = GameObject.Find ("Player");
		//beginPosition = transform.position;
		while (true)
		{
			yield return new WaitForSeconds(0.2f);
			if(allowMove){

				yield return StartCoroutine(MoveObject (transform, transform.position, endPosition, speed));
				//yield return StartCoroutine(MoveObject (transform, beginPosition, endPosition, 2f));
				if(transform.position.y >= endPosition.y) {
					break;
				}
			}

			//if(allowReset){
			//	yield return StartCoroutine(MoveObject (transform, transform.position, beginPosition, speed));
				//yield return StartCoroutine(MoveObject (transform, beginPosition, endPosition, 2f));
				//if(transform.position.y >= beginPosition.y) {
				//	allowReset = false;
					
				//}
			//}

		}
	}


	// Update is called once per frame
	void Update () {
	
	}

	//public void resetPosition() {
	//	transform.position = Vector3.Lerp(transform.position, beginPosition, speed);
	//}

	void OnCollisionEnter(Collision collisionInfo) {


		if (collisionInfo.gameObject.name.Equals ("Player")) {
			//Debug.Log(1);
			allowMove = true;
			//GameObject.Find("stage 1");
			//GameObject.Destroy(GameObject.Find("GroundCube"));
		}
	}
	/*
	void OnCollisionExit(Collision collisionInfo) {
		//Debug.Log (2);
		if (collisionInfo.gameObject.name.Equals ("Player")) {
			if(allowReset == true) {
				transform.position = Vector3.Lerp(transform.position, beginPosition, 1);
			//GameObject.Find("stage 1");
			//GameObject.Destroy(GameObject.Find("GroundCube"));
			}
		}
	}
*/

	IEnumerator MoveObject(Transform thisTransform, Vector3 startPos, Vector3 endPos, float time)
	{
		var i = 0.0f;
		var rate = 1.0f / time;
		while (i < 1.0f)
		{
			i += Time.deltaTime * rate;
			thisTransform.position = Vector3.Lerp(startPos, endPos, i);

			if(playerMove) {

				player.transform.position = Vector3.Lerp(startPos, endPos, i);
			}
			//thisTransform.Rotate(0,0,500*Time.deltaTime, Space.Self);
			yield return null;
		}
	}
	/*
	void OnCollisionExit(Collision collisionInfo) {
		if (collisionInfo.gameObject.name.Equals ("Player")) {
			gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, beginPosition, 1);
			gameObject.GetComponent<Renderer>().enabled = false;
		}
	}
	*/
}
