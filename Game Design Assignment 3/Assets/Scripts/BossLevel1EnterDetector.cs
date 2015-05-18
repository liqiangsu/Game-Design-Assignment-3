using UnityEngine;
using System.Collections;

public class BossLevel1EnterDetector : MonoBehaviour
{
    private BossLevel1 boss;
    [SerializeField]
    private AudioClip clip;

    private GameObject bgm;
    private AudioClip org;
    private AudioSource audio;
	// Use this for initialization
	void Start ()
	{
	    bgm = GameObject.Find("BGM");
	    audio = GetComponent<AudioSource>();
	    boss = GetComponentInParent<BossLevel1>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) { 
            boss.IsPlayerInArea = true;
            bgm.GetComponent<AudioSource>().Stop();
            audio.Play();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            bgm.GetComponent<AudioSource>().Play();
            audio.Stop();
            boss.IsPlayerInArea = false;
        }
        
    }
}
