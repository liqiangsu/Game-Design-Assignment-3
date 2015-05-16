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
    private GameObject player;
    private float lastPushTime = 0;
	// Use this for initialization
	void Start ()
	{
	    player = GameObject.FindGameObjectWithTag("Player");
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

			if(!IsPlayerOnSide(closest)){
				return;
			}
	        var dir = GetPushingDirection(closest);
            //closest.GetComponent<Rigidbody>().velocity = dir * PushForce;
            closest.GetComponent<Cube>().Move(dir);
	    }
	}

    private Vector3 GetPushingDirection(GameObject box)
    {
        var minX = box.transform.position.x - 0.5f;
        var minZ = box.transform.position.z - 0.5f;
        var maxX = box.transform.position.x + 0.5f;
        var maxZ = box.transform.position.z + 0.5f;
        // player position 
        var pp = player.transform.position;
                
        //up
        if (pp.z > maxZ)
        {
            return new Vector3(0,0,-1);
        }
        //down
        if (pp.z < minZ)
        {
            return new Vector3(0,0,1);
        }
        //left
        if (pp.x < minX)
        {
            return new Vector3(1,0,0);
        }
        //right
        if (pp.x > maxX)
        {
            return new Vector3(-1, 0, 0);
        }
        return Vector3.zero;
//        var normal = this.transform.forward;
//
//        return Mathf.Abs(normal.x) > Mathf.Abs(normal.z) ? new Vector3(normal.x < 0 ? -1 : 1, 0, 0) : new Vector3(0, 0, normal.z < 0 ? -1 : 1);
    }
	private bool IsPlayerOnSide(GameObject box){
		//0.5 should be half of the cube size
        var minX = box.transform.position.x - 0.5f;
        var minZ = box.transform.position.z - 0.5f;
        var maxX = box.transform.position.x + 0.5f;
        var maxZ = box.transform.position.z + 0.5f;

		var playerPosition = player.transform.position;
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
