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
        return new Vector3(
                            pp.x < minX ? 1 : pp.x > maxX ? -1 : 0, 
                            0,
                            pp.z < minZ ? 1 : pp.z > maxZ ? -1 : 0);
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
