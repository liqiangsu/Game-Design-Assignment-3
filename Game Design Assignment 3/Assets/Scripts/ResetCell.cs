using UnityEngine;
using System.Collections;

public class ResetCell : MonoBehaviour , IResetCell{

	public int GridX { get; set; }
	public int GridY { get; set; }
	
	public GameObject GameObject
	{
		get { return gameObject; }
	}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other){
		Debug.Log("trigger");
		Debug.Log (other);
		//if (other is ICell || other.CompareTag ("Player")) {
			Application.LoadLevel("Level1");
		//}
	}
}
