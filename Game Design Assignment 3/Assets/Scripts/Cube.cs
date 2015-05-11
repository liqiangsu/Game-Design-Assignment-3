using UnityEngine;

public class Cube : MonoBehaviour
{
    public float speed = 1;
    private bool IsMoved;

    private Vector3 targetPosition;
	// Use this for initialization
	void Start ()
	{
	}
	
	// Update is called once per frame
	void Update () {
	    if (IsMoved)
	    {
	        GetComponent<Rigidbody>().MovePosition(
                        Vector3.MoveTowards(transform.position, targetPosition,
	                    speed*Time.deltaTime));
	    }

	    if (transform.position == targetPosition)
	    {
	        IsMoved = false;
	    }
	}

    public void Move(Vector3 dir)
    {
        RaycastHit hit;
        //test pushing direction and upward direction if anything blocked
        var isHit = Physics.Raycast(new Ray(transform.position, dir), out hit, 1f) ||
                    Physics.Raycast(new Ray(transform.position, transform.up), out hit, 1f);
        if (!isHit)
        {
            targetPosition = transform.position + dir * 1;
            IsMoved = true;
        }
    }
}
