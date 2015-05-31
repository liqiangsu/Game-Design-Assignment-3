using System.Runtime.InteropServices;
using UnityEngine;

public class Cube : MonoBehaviour
{
    public float speed = 1;
    public bool IsMoved;

    private Vector3 targetPosition;

	private Vector3 pushDir;

    private GameObject grid;
    private Rigidbody rigi;
    [SerializeField]
    Material hightlightMaterial;
    Material orignalMeterial;

    [SerializeField] private AudioClip movingSoundClip;

	// Use this for initialization
	void Start ()
	{
	    grid = GameObject.Find("Grid");
	    rigi = this.GetComponent<Rigidbody>();
        orignalMeterial = GetComponent<Renderer>().material;

	}
	
	// Update is called once per frame
	void FixedUpdate () {
	    if (IsMoved)
	    {
	        rigi.useGravity = false;
	        rigi.MovePosition(
                        Vector3.MoveTowards(transform.position, targetPosition,
	                    speed*Time.deltaTime));
		}

	    if (IsMoved && transform.position.Equals2D(targetPosition))
	    {
			grid.GetComponent<Grid>().ForeceGrid();
	        IsMoved = false;
	        rigi.useGravity = true;
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
            AudioSource.PlayClipAtPoint(movingSoundClip, transform.position, 0.5f);
            targetPosition = transform.position + dir * 1;
            IsMoved = true;
			pushDir = dir;
            if (isHitUp)
            {
                hitUp.collider.gameObject.GetComponent<Cube>().Move(dir);
            }
        }
        else
        {
            GetComponent<Renderer>().material = hightlightMaterial;
            Invoke("ResetMaterial", 1f);
        }

    }

    void ResetMaterial()
    {
        GetComponent<Renderer>().material = orignalMeterial;
    }
}
