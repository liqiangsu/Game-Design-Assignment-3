using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityStandardAssets.CrossPlatformInput;

public class ExtraCharaterController : MonoBehaviour
{
    [SerializeField] float JumppingControlSpeed;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] float SmallMoveSpeed = 0.03f;
    [SerializeField] float RotatSpeed;
    private ThirdPersonCharacter character;
    private Rigidbody rigi;
	// Use this for initialization
	void Start ()
	{
	    character = GetComponent<ThirdPersonCharacter>();
	    rigi = GetComponent<Rigidbody>();
	    cameraTransform = GameObject.FindGameObjectWithTag("MainCamera").transform;
	}
	
	// Update is called once per frame
    void FixedUpdate() {
        //roatation to mouse
        // Generate a plane that intersects the transform's position with an upwards normal.
    	Plane playerPlane = new Plane(Vector3.up, transform.position);
 
    	// Generate a ray from the cursor position
    	Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
 
    	// Determine the point where the cursor ray intersects the plane.
    	// This will be the point that the object must look towards to be looking at the mouse.
    	// Raycasting to a Plane object only gives us a distance, so we'll have to take the distance,
    	//   then find the point along that ray that meets that distance.  This will be the point
    	//   to look at.
    	float hitdist = 0.0f;
    	// If the ray is parallel to the plane, Raycast will return false.
    	if (playerPlane.Raycast (ray, out hitdist)) 
		{
        	// Get the point along the ray that hits the calculated distance.
        	Vector3 targetPoint = ray.GetPoint(hitdist);
 
        	// Determine the target rotation.  This is the rotation if the transform looks at the target point.
        	Quaternion targetRotation = Quaternion.LookRotation(targetPoint - transform.position);
 
        	// Smoothly rotate towards the target point.
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, RotatSpeed * Time.deltaTime);
		}


        var h = CrossPlatformInputManager.GetAxis("Horizontal");
        var v = CrossPlatformInputManager.GetAxis("Vertical");
        if (character.m_IsGrounded && character.GetComponent<Rigidbody>().velocity.magnitude < 1)
        {
            v = v < 0 ? -1 : v > 0 ? 1 : 0;
            h = h < 0 ? -1 : h > 0 ? 1 : 0;
            var camForward = Vector3.Scale(cameraTransform.forward, new Vector3(1, 0, 1)).normalized;
            var move = v * camForward * SmallMoveSpeed + h * cameraTransform.right * SmallMoveSpeed;
            character.transform.position += move;
        }

        if (!character.m_IsGrounded)
        {
            var y = rigi.velocity.y;
            Vector3 move;
            if (cameraTransform != null)
            {
                // calculate camera relative direction to move:
                var camForward = Vector3.Scale(cameraTransform.forward, new Vector3(1, 0, 1)).normalized;
                move = v * camForward + h * cameraTransform.right;
            }
            else
            {
                // we use world-relative directions in the case of no main camera
                move = v * Vector3.forward + h * Vector3.right;
            }
            rigi.velocity = new Vector3(move.x * JumppingControlSpeed, y, move.z * JumppingControlSpeed );
        }
    }
}
