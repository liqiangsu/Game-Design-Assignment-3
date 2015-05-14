using System.Linq;
using UnityEngine;
using System.Collections;

public class RuneCompleteChecker : MonoBehaviour
{

    [SerializeField] private Transform[] Runes;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	    if (IsConnected())
	    {
            var boss = GameObject.Find("Boss");
            var cameraController = Camera.main.gameObject.GetComponent<FollowTarget>();
            cameraController.Target = boss.transform;
            boss.transform.FindChild("DestoryAnimation").gameObject.SetActive(true);
            Invoke("LoadLevel2", 3);
	    }
	}

    bool IsConnected()
    {
        //check completed
        //TODO: could be using better method
        float totalDistance = 0;
        for (int i = 0; i < Runes.Length-1; i++)
        {
            totalDistance += Vector3.Distance(Runes[i].position, Runes[i + 1].position);
        }

        return totalDistance <= 3.5;
    }

    void LoadLevel2()
    {
        Application.LoadLevel("Level2");
    }
}
