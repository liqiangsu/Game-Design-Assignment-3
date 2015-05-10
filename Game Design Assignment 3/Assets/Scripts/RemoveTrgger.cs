using UnityEngine;
using System.Collections;

public class RemoveTrgger : MonoBehaviour {
	[SerializeField] GameObject RemovingObject;
	[SerializeField] bool IsEnabled = true;
	[SerializeField] Vector3 ToPosition;
	[SerializeField] bool IsDestoryAfterMovment = true;

	private bool isTriggered = false;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (isTriggered && IsEnabled) {
			//just removing objects' collider, so it fall
			//TODO makeit slide to a position
			var rigi = RemovingObject.GetComponent<Rigidbody>();
			var collider = RemovingObject.GetComponent<Collider>();
			collider.enabled = false;
			rigi.constraints = RigidbodyConstraints.None;
			if(IsDestoryAfterMovment){
				Invoke("DestoryObject", 1f);
			}

			//disable this trigger afterit is been activated
			IsEnabled = false;
		}
	}

	void OnTriggerEnter(Collider other){
		if (other.CompareTag ("Player")) {
			isTriggered = true;
		}
	}

	void DestoryObject(){
		Destroy (RemovingObject);
	}
}
