using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityStandardAssets.CrossPlatformInput;

public class ExtraCharaterController : MonoBehaviour
{
    [SerializeField] float JumppingControlSpeed;
    [SerializeField] private Transform camera;
    private ThirdPersonCharacter character;
    private Rigidbody rigi;
	// Use this for initialization
	void Start ()
	{
	    character = GetComponent<ThirdPersonCharacter>();
	    rigi = GetComponent<Rigidbody>();
	    camera = GameObject.FindGameObjectWithTag("MainCamera").transform;
	}
	
	// Update is called once per frame
    void FixedUpdate() {
        if (!character.m_IsGrounded)
        {
            var y = rigi.velocity.y;
            var h = CrossPlatformInputManager.GetAxis("Horizontal");
            var v = CrossPlatformInputManager.GetAxis("Vertical");
            Vector3 move;
            if (camera != null)
            {
                // calculate camera relative direction to move:
                var camForward = Vector3.Scale(camera.forward, new Vector3(1, 0, 1)).normalized;
                move = v * camForward + h * camera.right;
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
