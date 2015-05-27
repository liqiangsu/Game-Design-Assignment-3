using UnityEngine;
using System.Collections;

public class BGMChanger : MonoBehaviour {

    [SerializeField]
    AudioClip Clip;
    AudioSource BGMSrouce;

    private AudioClip org;

    void Start()
    {
        BGMSrouce = GameObject.FindGameObjectWithTag("BGMSource").GetComponent<AudioSource>();
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            org = BGMSrouce.clip;
            BGMSrouce.clip = Clip;
            BGMSrouce.Play();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //BGMSrouce.clip = org;
        }
    }
}
