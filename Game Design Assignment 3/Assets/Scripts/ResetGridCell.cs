using UnityEngine;
using System.Collections;

public class ResetGridCell: MonoBehaviour{

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.R))
        {
            Application.LoadLevel("Level1");
        }
    }
	void OnTriggerEnter(Collider other){
		if (other.CompareTag ("Player")) {
			Application.LoadLevel("Level1");
		}
	}
}
