using UnityEngine;
using System.Collections;

public class BossLevel1EnterDetector : MonoBehaviour
{
    private BossLevel1 boss;
	// Use this for initialization
	void Start ()
	{
	    boss = GetComponentInParent<BossLevel1>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter()
    {
        boss.PlayerInArea = true;
    }

    void OnTriggerExist()
    {
        boss.PlayerInArea = false;
    }
}
