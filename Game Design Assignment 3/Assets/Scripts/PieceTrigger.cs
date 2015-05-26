using System;
using System.Linq;
using UnityEngine;
using System.Collections;
using UnityStandardAssets.Utility;

public class PieceTrigger : MonoBehaviour
{

    [SerializeField] private Material Deactive;
    [SerializeField] private Material Active;
    private bool isActivatied = false;
	// Use this for initialization
	void Start ()
	{
	}
	
	// Update is called once per frame
	void Update () {

	}

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isActivatied)
        {
            GetComponent<Renderer>().material = Active;
            GameObject.FindObjectOfType<SaveHelper>().Save();
            isActivatied = true;
        }
    }
}
