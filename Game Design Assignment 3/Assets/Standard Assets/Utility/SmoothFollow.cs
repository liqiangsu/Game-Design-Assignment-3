﻿using UnityEngine;

namespace Assets.Utility
{
	public class SmoothFollow : MonoBehaviour
	{

		// The target we are following
		[SerializeField]
		public Transform Target;
		// The distance in the x-z plane to the target
		[SerializeField]
		private float distance = 10.0f;
		// the height we want the camera to be above the target
		[SerializeField]
		private float height = 5.0f;

		[SerializeField]
		private float rotationDamping;
		[SerializeField]
		private float heightDamping;

        [SerializeField]
        float scrollSpeed = 1;

        Vector3 MouseStart;
        float dragDistance;
        [SerializeField]
        float rotateSpeed;
		// Use this for initialization
		void Start() { }

		// Update is called once per frame
		void LateUpdate()
		{
			// Early out if we don't have a target
			if (!Target)
				return;

            // rotate
            if (Input.GetMouseButtonDown(0))
            {
                MouseStart = Input.mousePosition;
            }
            if(Input.GetMouseButton(0)){
                dragDistance = MouseStart.x - Input.mousePosition.x;
                transform.Rotate(new Vector3(0, dragDistance * rotateSpeed * Time.deltaTime), Space.World);
            }


            //change ceamra height by scorll
            height += Input.GetAxis("Mouse ScrollWheel") * scrollSpeed;
            if (height < 0)
            {
                height = 0;
            }
            if (height > 10)
            {
                height = 10;
            }

			// Calculate the current rotation angles
			var wantedRotationAngle = Target.eulerAngles.y;
			var wantedHeight = Target.position.y + height;

			var currentRotationAngle = transform.eulerAngles.y;
			var currentHeight = transform.position.y;

			// Damp the rotation around the y-axis
			currentRotationAngle = Mathf.LerpAngle(currentRotationAngle, wantedRotationAngle, rotationDamping * Time.deltaTime);

			// Damp the height
			currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightDamping * Time.deltaTime);

			// Convert the angle into a rotation
			var currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);

			// Set the position of the camera on the x-z plane to:
			// distance meters behind the target
			transform.position = Target.position;
			transform.position -= currentRotation * Vector3.forward * distance;

			// Set the height of the camera
			transform.position = new Vector3(transform.position.x ,currentHeight , transform.position.z);

			// Always look at the target
			transform.LookAt(Target);
            

		}
	}


}