using System.Runtime.InteropServices;
using UnityEngine;

public class Cube : MonoBehaviour
{
    public float speed = 1;
    private bool IsMoved;

    private Vector3 targetPosition;

	private Vector3 pushDir;

    private GameObject grid;
    private Rigidbody rigi;
	// Use this for initialization
	void Start ()
	{
	    grid = GameObject.Find("Grid");
	    rigi = this.GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
	    if (name == "Cube 45")
	    {
	        //Debug.Log(GetComponent<Rigidbody>().velocity);
	    }
	    if (IsMoved)
	    {
	        GetComponent<Rigidbody>().MovePosition(
                        Vector3.MoveTowards(transform.position, targetPosition,
	                    speed*Time.deltaTime));
		}

	    if (IsMoved && transform.position.Equals2D(targetPosition))
	    {
			grid.GetComponent<Grid>().ForeceGrid();
	        IsMoved = false;
	    }
	}
    public void Move(Vector3 dir)
    {
        RaycastHit hit;
        RaycastHit hitUp;
        // make cube on top move to direction as well
        var isHitForward = Physics.Raycast(new Ray(transform.position, dir), out hit, 1f);
        var isHitUp = Physics.Raycast(new Ray(transform.position, transform.up), out hitUp, 1f);
        if (!isHitForward || hit.collider.gameObject.CompareTag("PutOnTrigger"))
        {
            targetPosition = transform.position + dir * 1;
            IsMoved = true;
			pushDir = dir;
            if (isHitUp)
            {
                hitUp.collider.gameObject.GetComponent<Cube>().Move(dir);
            }
        }

    }
}
