using System;
using System.Linq;
using UnityEngine;
using System.Collections;

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
            var controller = Camera.main.GetComponent<SmoothFollow>();
            controller.target = target.transform;
        }
    }

    
}
