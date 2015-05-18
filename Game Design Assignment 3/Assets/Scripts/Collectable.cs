using System;
using UnityEditor;
using UnityEngine;
using System.Collections;

public class Collectable : MonoBehaviour
{
    private CollectionManager collectionManager;
    private GameObject Player;
    [SerializeField] public float MagnetDistance = 1;
    [SerializeField] public float MagnetSpeed = 2;

    private AudioSource audio;
	// Use this for initialization
	void Start ()
	{
	    Player = GameObject.FindGameObjectWithTag("Player");
	    audio = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
//	void FixedUpdate () {
//	    if (IsPlayerNearBy())
//	    {
//	        transform.position = Vector3.MoveTowards(transform.position, Player.transform.FindChild("PushCenter").position,
//	            MagnetSpeed*Time.deltaTime);
//	    }
//	}
//
//    bool IsPlayerNearBy()
//    {
//        var distance = Vector3.Distance(transform.position, Player.transform.position);
//        return distance < MagnetDistance;
//    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CollectionManager.MagicCount++;
            AudioSource.PlayClipAtPoint(audio.clip, transform.position, 1f);
			Destroy(this.gameObject);
        }
    }
}
