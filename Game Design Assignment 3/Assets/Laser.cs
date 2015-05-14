using System.Security.Cryptography;
using UnityEngine;
using System.Collections;

public class Laser : MonoBehaviour
{
    private LineRenderer lineRenderer;
    [SerializeField]
    private float laserHight = 20;
    [SerializeField] private GameObject LaserBurn;
	// Use this for initialization
	void Start ()
	{
	    lineRenderer = GetComponent<LineRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
	    Ray ray = new Ray(transform.position + new Vector3(0,laserHight,0), -transform.up);
	    RaycastHit hitInfo;
	    if (Physics.Raycast(ray,out hitInfo, laserHight + 1))
	    {
            Instantiate(LaserBurn, hitInfo.point + new Vector3(0,0.1f,0), Quaternion.identity);
	    }
        lineRenderer.SetPosition(1, hitInfo.point);
	}
}
