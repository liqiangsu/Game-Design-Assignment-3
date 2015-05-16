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
	    if (gameObject.name == "Cube 29")
	    {
	        Debug.Log(gameObject.transform.position);
	    }
	    if (IsMoved)
	    {
	        GetComponent<Rigidbody>().MovePosition(
                        Vector3.MoveTowards(transform.position, targetPosition,
	                    speed*Time.deltaTime));

			//transform.position += pushDir;
			//GetComponent<Rigidbody>().AddForce(pushDir * speed);
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
        //test pushing direction and upward direction if anything blocked
        var isHit = Physics.Raycast(new Ray(transform.position, dir), out hit, 1f) ||
                    Physics.Raycast(new Ray(transform.position, transform.up), out hit, 1f);
        if (!isHit || (isHit && hit.collider.gameObject.CompareTag("PutOnTrigger")))
        {
            targetPosition = transform.position + dir * 1;
            IsMoved = true;
			pushDir = dir;
        }
    }
}
