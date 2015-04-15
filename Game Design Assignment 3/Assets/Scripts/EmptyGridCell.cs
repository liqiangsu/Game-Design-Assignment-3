using UnityEngine;

public class EmptyGridCell : GridCell {
    // Use this for initialization
	void Start ()
	{
	    GetComponent<Renderer>().enabled = false;
	}
	
}