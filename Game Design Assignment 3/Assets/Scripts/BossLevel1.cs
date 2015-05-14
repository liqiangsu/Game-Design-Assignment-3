﻿using System;
using UnityEngine;
using System.Collections;

public class BossLevel1 : MonoBehaviour
{
    //Mean to be modify by external triigers
    //BossLevel1EnterDetector for example
    public bool PlayerInArea;
    
    public bool IsFireEnabled = false;
    public bool IsFireSmallEnabled = false;
    [SerializeField] private float FireIntervelTime = 5;
    [SerializeField] private float SmallFireIntervalTime = 0.2f;
    private float lastFiredTime = 0;
    private float lastSmallFireTime = 0;

    public GameObject Player;
    public GameObject FirePrefab;

    private GameObject theCamera;
    // Use this for initialization
	void Start ()
	{
	    Player = GameObject.FindGameObjectWithTag("Player");
	    theCamera = Camera.main.gameObject;
	}

    void Change()
    {
        theCamera.GetComponent<FollowTarget>().Target = this.gameObject.transform;

    }
	// Update is called once per frame
	void Update () {
	    if (PlayerInArea && Player)
	    {
	        if (IsFireEnabled && Time.time - lastFiredTime > FireIntervelTime)
	        {
                //shake Camera
                theCamera.GetComponent<CameraShake>().Shake();

                //TODO: change size of fire, could use prefab instead
	            var instance = Instantiate(FirePrefab, Player.transform.position, Quaternion.identity) as GameObject;
                if (instance)
                    instance.transform.localScale = new Vector3(2f, 0f, 2f);
	            lastFiredTime = Time.time;
	        }
            if (IsFireSmallEnabled && Time.time - lastSmallFireTime > SmallFireIntervalTime)
	        {
	            var instance = Instantiate(FirePrefab, Player.transform.position, Quaternion.identity) as GameObject;
	            if(instance)
                    instance.transform.localScale = new Vector3(0.3f,0f,0.3f);
	            lastSmallFireTime = Time.time;
	        }
	    }
	}


    public void Die()
    {
        //animation should be deactive in editor
        var go = transform.FindChild("DestoryAnimation");
        go.gameObject.SetActive(true);
        new WaitForSeconds(2);

    }

}