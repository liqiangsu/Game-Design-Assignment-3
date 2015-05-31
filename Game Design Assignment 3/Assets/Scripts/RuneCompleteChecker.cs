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
	        if (Application.loadedLevelName == "Level1")
	        {
	            cameraController.Target = boss.transform;
	            boss.transform.FindChild("DestoryAnimation").gameObject.SetActive(true);
	            Invoke("LoadLevel2", 3);
	        }
	        if (Application.loadedLevelName == "Level2")
	        {
	            var teleporter = transform.Find("FinalTeleporter");
                teleporter.gameObject.SetActive(true);
	        }
	        if (Application.loadedLevelName == "Level3")
	        {
                cameraController.Target = boss.transform;
                boss.transform.FindChild("DestoryAnimation").gameObject.SetActive(true);
                Invoke("LoadEndGame", 3);
	        }
	    }
	}

    bool IsConnected()
    {
        //check completed
        //TODO: could be using better method
//        float totalDistance = 0;
//        for (int i = 0; i < Runes.Length-1; i++)
//        {
//            totalDistance += Vector3.Distance(Runes[i].position, Runes[i + 1].position);
//        }
//
        bool isConnected = false;
        float rayCastDistance = 0.6f;

        bool c2, c3, c4;
        bool c1 = c2 = c3 = c4 = false;
        RaycastHit hitInfo1;
        if (Physics.Raycast(Runes[0].position, Vector3.right, out hitInfo1, rayCastDistance))
        {
            c1 = hitInfo1.collider.gameObject == Runes[1].gameObject;
        }
        RaycastHit hitInfo2;
        if (Physics.Raycast(Runes[0].position, Vector3.back, out hitInfo2, rayCastDistance))
        {
            c2 = hitInfo2.collider.gameObject == Runes[2].gameObject;
        }
        RaycastHit hitInfo3;
        if (Physics.Raycast(Runes[3].position, Vector3.forward, out hitInfo3, rayCastDistance))
        {
            c3 = hitInfo3.collider.gameObject == Runes[1].gameObject;
        }
        RaycastHit hitInfo4;
        if (Physics.Raycast(Runes[3].position, Vector3.left, out hitInfo4, rayCastDistance))
        {
            c4 = hitInfo4.collider.gameObject == Runes[2].gameObject;
        }
        isConnected = c1 && c2 && c3 && c4;
        return isConnected;
        //return totalDistance <= 3.5;
    }

    void LoadLevel2()
    {
        Application.LoadLevel("Level2");
    }

    void LoadEndGame()
    {
        Application.LoadLevel("EndGame");

    }
}
