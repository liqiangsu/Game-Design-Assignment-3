using System.Linq;
using UnityEngine;
using System.Collections;

public class RuneCompleteChecker : MonoBehaviour
{

    [SerializeField] public Transform[] Runes;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	    IsConnected();
	}

    void IsConnected()
    {
        float totalDistance = 0;
        for (int i = 0; i < Runes.Length-1; i++)
        {
            totalDistance += Vector3.Distance(Runes[i].position, Runes[i + 1].position);
        }
        Debug.Log(totalDistance);
        if (totalDistance <= 3.5)
        {
            Debug.Log("completed");
        }
    }
}
