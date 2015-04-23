using System;
using UnityEngine;
using System.Collections;

public class ExitGridCell : MonoBehaviour{
  
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Invoke("LoadLevel", 0.5f);
        }
    }

    void LoadLevel()
    {
        Application.LoadLevel("EndGame");
    }

}