using UnityEngine;
using System.Collections;

public class TestMove : MonoBehaviour
{


    [SerializeField] private Vector3 dirction = new Vector3();
    [SerializeField] private float speed  = 1;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	     transform.Translate(dirction * speed * Time.deltaTime, Space.World);
	}

}
