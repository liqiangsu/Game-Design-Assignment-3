using UnityEngine;
using System.Collections;

public class TimerMovingGround : MonoBehaviour
{
    [SerializeField]
    float speed;
    [SerializeField]
    Vector3 direction;
    public bool IsStoped;

    SaveHelper saveHelper;
	// Use this for initialization
	void Start () {
        saveHelper = GameObject.FindObjectOfType<SaveHelper>();
	}
	
	// Update is called once per frame
	void Update () {
        transform.Translate(direction * speed * Time.deltaTime, Space.World);       
    }

    void Move()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            saveHelper.Load();
        }
    }
}
