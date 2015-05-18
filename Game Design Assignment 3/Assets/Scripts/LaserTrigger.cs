using UnityEngine;
using System.Collections;

public class LaserTrigger : MonoBehaviour
{

    private bool isActivated = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isActivated)
        {
            transform.FindChild("Particle").gameObject.SetActive(false);
            isActivated = true;
        }
    }
}
