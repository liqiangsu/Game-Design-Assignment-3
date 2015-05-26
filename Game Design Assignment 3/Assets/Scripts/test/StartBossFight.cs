using UnityEngine;
using System.Collections;

public class StartBossFight : MonoBehaviour {
	//int count = 0;
	// Use this for initialization
	void Start () {

		//mb.enabled = true;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerStay(Collider other) {

		if (other.gameObject.name.Equals ("Player")) {
			//Debug.Log (GameObject.Find ("Boss_lvl2").name);
			GameObject.Find ("Boss_lvl2").GetComponent <Boss> ().enabled = true;
			//GameObject.Find ("Boss_lvl2").GetComponent <Boss> ().fightFlag = true;
			//Debug.Log ("start--------------");
		}
	}

}
