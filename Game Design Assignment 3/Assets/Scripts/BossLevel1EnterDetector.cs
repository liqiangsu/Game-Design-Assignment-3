using UnityEngine;
using System.Collections;

public class BossLevel1EnterDetector : MonoBehaviour
{
    private BossLevel1 boss;
    [SerializeField]
	// Use this for initialization
	void Start ()
	{
	    boss = GameObject.FindObjectOfType<BossLevel1>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) { 
            boss.IsPlayerInArea = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            boss.IsPlayerInArea = false;
        }
        
    }
}
