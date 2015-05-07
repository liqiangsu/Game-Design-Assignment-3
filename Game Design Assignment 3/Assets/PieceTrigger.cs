using System;
using System.Linq;
using UnityEngine;
using System.Collections;
using UnityStandardAssets.Utility;

public class PieceTrigger : MonoBehaviour
{

    private Animator animator;
    [SerializeField] private GameObject target;
    [SerializeField] private GameObject _camera;
	// Use this for initialization
	void Start ()
	{
	    animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {

	}

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            animator.SetBool("IsTriggered", true);
            Camera.main.GetComponent<FollowTarget>().Target = target.transform;
            target.GetComponent<Rigidbody>().useGravity = true;
            Invoke("ReSetCamera", 2f);
        }
    }

    void ReSetCamera()
    {
        Camera.main.GetComponent<FollowTarget>().Target = GameObject.FindGameObjectWithTag("Player").transform;
        GameObject.Find("SavePoint").GetComponent<SaveHelper>().Save();
        Destroy(this);
    }

    
}
