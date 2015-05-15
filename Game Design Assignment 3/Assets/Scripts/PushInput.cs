using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters;
using UnityEditor;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PushInput : MonoBehaviour
{
    public bool IsPushState;
    public float GrappingDistance;
    public float GrappingAngle;
    public float PushForce = 5;

    private float lastPushTime = 0;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
		IsPushState = Input.GetButtonDown("Push");
	    if (IsPushState)
	    {
            // do nothing when last push is too quick
            // to preven cubes force out position
            if (Time.time - lastPushTime < 0.3)
            {
                return;
            }
            lastPushTime = Time.time;
            var closest = FindFacingCubeRayCast();
	        if (closest == null)
	        {
	            return;
	        }

			if(!IsPlayerOnSide()){
				return;
			}
	        var normal = this.transform.forward;
            var dir = Mathf.Abs(normal.x) > Mathf.Abs(normal.z) ? new Vector3(normal.x < 0? -1 : 1, 0, 0) : new Vector3(0, 0, normal.z < 0 ? -1: 1);
            //closest.GetComponent<Rigidbody>().velocity = dir * PushForce;
            closest.GetComponent<Cube>().Move(dir);
	    }
	}
	
	private bool IsPlayerOnSide(){
		//0.5 should be half of the cube size
		var minX = transform.position.x - 0.5f;
		var minZ = transform.position.z - 0.5f;
		var maxX = transform.position.x + 0.5f;
		var maxZ = transform.position.z + 0.5f;

		var playerPosition = GameObject.FindGameObjectWithTag ("Player").transform.position;
		return (playerPosition.x > minX && playerPosition.x < maxX) || 
			(playerPosition.z > minZ && playerPosition.z < maxZ);

	}
	private GameObject FindFacingCubeRayCast(){
		RaycastHit hit;
        var isHit = Physics.Raycast(new Ray(transform.position, transform.forward), out hit, GrappingDistance);
	    if (isHit)
	    {
	        return hit.collider.gameObject;
	    }
	    return null;
	}

}
