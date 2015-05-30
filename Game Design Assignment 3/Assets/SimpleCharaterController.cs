using System.Runtime.InteropServices;
using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class SimpleCharaterController : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 1;
    [SerializeField] private float turnSpeed = 100;
    private Transform cam;
	// Use this for initialization
	void Start ()
	{
	    cam = Camera.main.transform;
	}
	
	// Update is called once per frame
    void FixedUpdate()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 move;
        // calculate move direction to pass to character
        if (cam != null)
        {
            // calculate camera relative direction to move:
            var camForward = Vector3.Scale(cam.forward, new Vector3(1, 0, 1)).normalized;
            move = v * camForward + h * cam.right;
        }
        else
        {
            // we use world-relative directions in the case of no main camera
            move = v * Vector3.forward + h * Vector3.right;
        }
        transform.position += move * movementSpeed * Time.deltaTime;


        move.Normalize();
        move = transform.InverseTransformDirection(move);
        move = Vector3.ProjectOnPlane(move, Vector3.up);
        var m_TurnAmount = Mathf.Atan2(move.x, move.z);
        transform.Rotate(0,m_TurnAmount * turnSpeed * Time.deltaTime,0);
    }
}
