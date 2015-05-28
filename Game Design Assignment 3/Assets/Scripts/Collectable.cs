using UnityEngine;

public class Collectable : MonoBehaviour
{
    [SerializeField] public float MagicWorth = 1;

    private AudioSource audioSource;
	// Use this for initialization
	void Start ()
	{
	    audioSource = GetComponent<AudioSource>();
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
            CollectionManager.CollectMagic(MagicWorth);
            AudioSource.PlayClipAtPoint(audioSource.clip, transform.position, 1f);
			Destroy(this.gameObject);
        }
    }
}
