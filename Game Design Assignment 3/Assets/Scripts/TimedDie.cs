using UnityEngine;
using System.Collections;

public class TimedDie : MonoBehaviour {

	// Use this for initialization
	void Start () {
	    Invoke("Die", 2.5f);    
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void Die()
    {
        Destroy(gameObject);
    }
    
}
