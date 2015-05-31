using UnityEngine;
using System.Collections;

public class FinalTeleporter : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(0,30,0));
            Invoke("LoadLevel3", 3);
        } 
    }

    void LoadLevel3()
    {
        Application.LoadLevel("Level3");
    }
}
