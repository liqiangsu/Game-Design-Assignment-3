using System;
using UnityEngine;
using System.Collections;

public class ExitCell : MonoBehaviour, IExitCell
{
    public int GridX { get; set; }
    public int GridY { get; set; }

    public GameObject GameObject
    {
        get { return gameObject; }
    }

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