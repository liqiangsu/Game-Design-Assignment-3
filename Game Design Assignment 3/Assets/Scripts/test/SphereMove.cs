using UnityEngine;
using System.Collections;

public class SphereMove : MonoBehaviour {


	[SerializeField]
	public Vector3 BeginPosition;
	public Vector3 EndPositiosn;
	public float TimeToFinsih;
	public float Delay;
	
	
	IEnumerator Start()
	{

		//BeginPosition = transform.position;
		//EndPositiosn = transform.FindChild("MovingTarget").position;
		while (true)
		{
			yield return StartCoroutine(MoveObject(transform, BeginPosition, EndPositiosn, TimeToFinsih));
			yield return new WaitForSeconds(Delay);
			yield return StartCoroutine(MoveObject(transform, EndPositiosn, BeginPosition, TimeToFinsih));
			yield return new WaitForSeconds(Delay);
		}
	}
	
	IEnumerator MoveObject(Transform thisTransform, Vector3 startPos, Vector3 endPos, float time)
	{
		var i = 0.0f;
		var rate = 1.0f / time;
		while (i < 1.0f)
		{
			i += Time.deltaTime * rate;
			thisTransform.position = Vector3.Lerp(startPos, endPos, i);
			//thisTransform.Rotate(0,0,500*Time.deltaTime, Space.Self);
			yield return null;
		}
	}

	/**
	private bool turn = false;
	[SerializeField]
	public Vector3 EndPositiosn;
	public Vector3 BeginPosition;
	public float speed;
	
	//public float Delay;

	// Use this for initialization
	IEnumerator Start()
	{
		while (true)
		{
			yield return StartCoroutine(MoveObject());

		}
	}
	
	IEnumerator MoveObject() 
	{

		if (BeginPosition.x >= transform.position.x) {
			turn = false;
		} else if (EndPositiosn.x <= transform.position.x) {
			turn = true;
		}
		Transform tf = gameObject.transform;
		if (turn) {
			
			tf.Translate (Vector3.left * Time.deltaTime * speed);
			
		} else {
			
			tf.Translate (Vector3.right * Time.deltaTime * speed);
			
		}
		yield return null;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
*/

}
