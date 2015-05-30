using UnityEngine;
using System.Collections;

public class InGameMenu : MonoBehaviour
{

    private bool isActive;
	// Use this for initialization
	void Start () {
	    DeactivateMenuItems();
	}
	
	// Update is called once per frame
	void Update () {
	    if (Input.GetButtonDown("Cancel") && !isActive)
	    {
	        ActivateMenuItems();
	        isActive = true;
	    }
	    else if(Input.GetButtonDown("Cancel"))
	    {
	        DeactivateMenuItems();
	        isActive = false;
	    }
	}

    public void ActivateMenuItems()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i);
            child.gameObject.SetActive(true);
        }
        Time.timeScale = 0;
    }

    public void DeactivateMenuItems()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i);
            child.gameObject.SetActive(false);
        }
        Time.timeScale = 1;
    }
}
