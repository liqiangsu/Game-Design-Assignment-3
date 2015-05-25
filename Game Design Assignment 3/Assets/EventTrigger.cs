using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EventTrigger : MonoBehaviour
{

    [SerializeField] [Multiline] private string Message;
    [SerializeField] private float TimeOut;
    [SerializeField] private EventTriggerTextControl TextControl;
    private bool isActivated = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        if (isActivated)
        {
            return;
        }
        if (other.CompareTag("Player")) { 
            TextControl.Activate(Message, TimeOut);
            isActivated = true;
        }
    }
}
