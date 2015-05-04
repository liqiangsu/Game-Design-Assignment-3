using UnityEngine;
using System.Collections;

public class TimerMovingGround : MonoBehaviour
{
    public float IntervalTimeInSecond = 3;

    public bool IsStoped;
	// Use this for initialization
	void Start () {
	    InvokeRepeating("Move",IntervalTimeInSecond,1);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void Move()
    {
        var old = transform.position;
        transform.position = new Vector3(old.x, old.y, old.z +1);
    }
}
