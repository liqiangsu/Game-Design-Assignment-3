using UnityEngine;
using System.Collections;

public class ResetGridCell: GridCell{


	void OnTriggerEnter(Collider other){
		Debug.Log("trigger");
		Debug.Log (other);
		//if (other is ICell || other.CompareTag ("Player")) {
			Application.LoadLevel("Level1");
		//}
	}
}
