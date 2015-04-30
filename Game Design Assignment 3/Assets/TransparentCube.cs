using System;
using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;
public class TransparentCube : MonoBehaviour
{

    private Animation animation;
	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    void RandomDelay(Animation animation)
    {
        this.animation = animation;
        animation.Stop();

        Invoke("ResumeAnimation", Random.Range(0, 3));
    }

    void ResumeAnimation()
    {
        animation.Play();
    }

}
