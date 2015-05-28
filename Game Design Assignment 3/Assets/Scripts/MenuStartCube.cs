using UnityEngine;
using System.Collections;

public class MenuStartCube : MonoBehaviour
{

    private AudioSource audioSource;
    private Vector3 orgPosition;
    private Vector3 targetPosition;
    [SerializeField]
    private float speed;
	// Use this for initialization
	void Start ()
	{
	    targetPosition = orgPosition = transform.position;
	    audioSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
	    if (transform.position != targetPosition)
	    {
	        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed);
	    }
	}

    void OnMouseEnter()
    {
        targetPosition = orgPosition + new Vector3(0, 0, -1);

        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }
    void OnMouseOver()
    {
        if(Input.GetMouseButtonDown(0) || Input.GetButtonDown("Submit")){
            Application.LoadLevel("Map");
        }
    }

    void OnMouseExit()
    {
        targetPosition = orgPosition;
    }
}
