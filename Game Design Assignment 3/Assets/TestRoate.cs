using UnityEngine;
using System.Collections;

public class TestRoate : MonoBehaviour
{
    [SerializeField]
    private Transform target;
    [SerializeField]
    private float time;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    transform.RotateAround(target.position, transform.up, (360 /  time) * Time.deltaTime);
	}
}
