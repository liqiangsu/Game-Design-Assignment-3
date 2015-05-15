using UnityEngine;
using System.Collections;

public class DieOnFire : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnParticleCollision(GameObject other)
    {
        Debug.Log(other.tag);
        if (other.CompareTag("DamagingParticle"))
        {
			FindObjectOfType<SaveHelper>().Load();
        }
    }
}
