using UnityEngine;
using System.Collections;

public class MoveGround : MonoBehaviour {
	private Vector3 t;
	[SerializeField]
	private float speed;

	IEnumerator Start()
	{
		Vector3 t = transform.position;

		while (true)
		{
			yield return new WaitForSeconds(speed);
			yield return StartCoroutine(MoveObject(transform, transform.position, t, speed));

		}
	}

	IEnumerator MoveObject(Transform tf, Vector3 beginPos, Vector3 endPos, float time) 
	{
		t.z -= 0.0833f;
		tf.position = Vector3.Lerp(beginPos, endPos, time);
		yield return null;
	}
}
