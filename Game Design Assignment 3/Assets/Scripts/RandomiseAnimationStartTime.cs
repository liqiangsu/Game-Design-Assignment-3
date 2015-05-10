using System;
using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Animator))]
public class RandomiseAnimationStartTime : MonoBehaviour
{

    private Animator animator;
	// Use this for initialization
	void Start ()
	{
	    animator = GetComponent<Animator>();
	    animator.enabled = false;
        Invoke("ReEnable", Random.insideUnitSphere.magnitude * 3);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void ReEnable()
    {
        animator.enabled = true;
    }
}
