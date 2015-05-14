using UnityEngine;
using System.Collections;

public class LaserBurn : MonoBehaviour
{

    private float lifeTime = 2;
	// Use this for initialization
	void Start () {
	    Invoke("Die", lifeTime);
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    void Die()
    {
        Destroy(this.gameObject);
    }
}
