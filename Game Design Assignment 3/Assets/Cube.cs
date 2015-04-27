using UnityEngine;
using System.Collections;

public class Cube : MonoBehaviour
{

    private bool IsMoved;
	// Use this for initialization
	void Start ()
	{
	}
	
	// Update is called once per frame
	void Update () {
	    if (GetComponent<Rigidbody>().velocity.magnitude >= 1)
	    {
	        IsMoved = true;
	    }
	    if (IsMoved && GetComponent<Rigidbody>().velocity.magnitude < 0.01)
	    {
            FindObjectOfType<Grid>().ForeceGrid();
	        IsMoved = false;
	    }
	}
}
