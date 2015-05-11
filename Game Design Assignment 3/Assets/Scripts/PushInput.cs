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
    public GameObject GrappedGameObject;
    public float GrappingAngle;
    public float PushForce = 5;

    private float lastPushTime = 0;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
	    IsPushState = Input.GetKeyDown(KeyCode.E);
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
            //debug
	        if (GrappedGameObject == null)
	        {
	            GrappedGameObject = closest;
	        }
	        var normal = this.transform.forward;
            var dir = Mathf.Abs(normal.x) > Mathf.Abs(normal.z) ? new Vector3(normal.x < 0? -1 : 1, 0, 0) : new Vector3(0, 0, normal.z < 0 ? -1: 1);
            //closest.GetComponent<Rigidbody>().velocity = dir * PushForce;
            closest.GetComponent<Cube>().Move(dir);
	    }else
	    {
	        GrappedGameObject = null;
	    }
	}

    private GameObject FindClosestCube(GameObject[] gameObjects)
    {
        try
        {
            return gameObjects.OrderBy(o => (Vector3.Distance(o.transform.position, this.transform.position))).First();

        }
        catch (Exception)
        {
            
            return null;
        }
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


    private GameObject FindFacingCubes()
    {
        Vector3 position = transform.Find("PushCenter").transform.position;
        var obs = GameObject.FindGameObjectsWithTag("Cube").ToList();
        var objectsInRange =
            obs.Where(o => Vector3.Distance(o.transform.position, position) < GrappingDistance);

        if (!objectsInRange.Any())
        {
            return null;
        }
        var objectsInView = objectsInRange.Where(o => (Vector3.Angle((o.transform.position - position), this.transform.forward) < GrappingAngle));

        if (!objectsInView.Any() || !objectsInRange.Any())
        {
            return null;
        }

        var nearestObject = objectsInRange.OrderBy(o => (Vector3.Distance(o.transform.position, position))).First();

        var mostDirectViewingObject = objectsInView.OrderBy(o => (Vector3.Angle((o.transform.position - position), this.transform.forward))).First();

        return nearestObject != mostDirectViewingObject ? null : nearestObject;
    }
}
