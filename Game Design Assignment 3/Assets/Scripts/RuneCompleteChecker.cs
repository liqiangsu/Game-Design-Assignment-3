using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;
using System.Collections;
public class RuneCompleteChecker : MonoBehaviour
{

    [SerializeField] private Transform[] Runes;
    private GameObject boss;
    private Assets.Utility.SmoothFollow cameraController;
	// Use this for initialization
	void Start () {
        boss = GameObject.Find("Boss");
        cameraController = Camera.main.gameObject.GetComponent<Assets.Utility.SmoothFollow>();
	}
	
	// Update is called once per frame
	void Update ()
	{
	    if (IsConnected())
	    {
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
