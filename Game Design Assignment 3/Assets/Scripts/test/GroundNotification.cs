using UnityEngine;
using System.Collections;

public class GroundNotification : MonoBehaviour {
	
	[SerializeField]
	private GameObject lightingObject;
	/*
	[SerializeField]
	private float Delay = 0.5f;
	[SerializeField]
	private int times = 5;
*/

	void OnTriggerEnter(Collider collider){
		if(collider.gameObject.name.Equals("Player")) {
			GameObject light = GameObject.Find ("TargetLight");

			Vector3 targetPos = new Vector3();
			targetPos.x = lightingObject.transform.position.x;
			targetPos.z = lightingObject.transform.position.z;
			targetPos.y = light.transform.position.y;
			/*
			//Vector3 hiddenPos = targetPos;
			//hiddenPos.y -= 10;
			//for(int i = 0; i < times; i ++) {*/
			light.transform.position = targetPos;
				//yield return new WaitForSeconds(Delay);
				//light.transform.position = hiddenPos;
				//yield return new WaitForSeconds(Delay);
			//}

		}
	}

}
