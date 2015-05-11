using UnityEngine;
using System.Collections;

public class MagicSystem : MonoBehaviour {

	[SerializeField] float EffectiveDraggingDistance;
	Vector3 initalMouseDragPosition;
	[SerializeField] int MouseButton = 0;
	GameObject selectedObject = null;


	private float DragedDistance{
		get{
			if (Input.GetMouseButton(MouseButton) && initalMouseDragPosition != Vector3.zero) {
				var CurrentMousePosition = Input.mousePosition;
				var distance = Vector3.Distance(initalMouseDragPosition, CurrentMousePosition);
				return distance;
			}
			return 0;
		}
	}

	private Vector3 DragedDirecton{
		get{
			if(Input.GetMouseButton(MouseButton)){
				var convertedMousePosition = new Vector3(Input.mousePosition.x,0,Input.mousePosition.y);
				var heading = convertedMousePosition - initalMouseDragPosition;
				var distance = heading.magnitude;
				if(distance == 0){
					return Vector3.zero;
				}
				Vector3 direction = heading / distance;
				return direction;
			}
			return Vector3.zero;
		}
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		//select by mouse
		if (Input.GetMouseButtonDown (MouseButton)) {
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			if (Physics.Raycast (ray, out hit, 100.0f)) {
				hit.collider.transform.tag = "Cube";

				var selectedGO = hit.collider.gameObject;
				//debug
				selectedGO.GetComponent<Renderer> ().material.color = Color.red;

				selectedObject = selectedGO;
				initalMouseDragPosition = Input.mousePosition;
			}
		}
		if (Input.GetMouseButtonUp (0)) {
			initalMouseDragPosition = Vector3.zero;
		}

		if (selectedObject && Input.GetMouseButton (MouseButton)) {
			Debug.Log(DragedDistance);
			selectedObject.GetComponent<Cube>().Move(DragedDirecton);

		}

	}
}
