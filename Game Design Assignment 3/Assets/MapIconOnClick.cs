using System;
using UnityEngine;
using System.Collections;

public class MapIconOnClick : MonoBehaviour
{
    public String LevelName;
	// Use this for initialization

    void OnMouseUp()
    {
        Application.LoadLevel(LevelName);
    }
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
