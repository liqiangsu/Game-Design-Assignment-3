using System.Runtime.Remoting.Services;
using UnityEditor;
using UnityEngine;
using System.Collections;
using UnityEngine.iOS;

public class MapIconHightlightAnimator : MonoBehaviour
{
    private ParticleSystem ps;

    public ParticleSystem rain;
    public Mesh Mesh;
	// Use this for initialization
	void Start ()
	{
	    ps = GetComponentInChildren<ParticleSystem>();
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    void OnMouseEnter()
    {
        Debug.Log("ps " + rain);
        ps.Play();
        rain.GetComponent<ParticleSystemRenderer>().mesh = Mesh;
    }
    void OnMouseExit()
    {
        ps.Stop();
    }
}
