using UnityEngine;

public class MapIconHightlightAnimator : MonoBehaviour
{
    private ParticleSystem childParticleSystem;

    public ParticleSystem rain;
    public Mesh Mesh;
    private AudioSource audioSource;
	// Use this for initialization
	void Start ()
	{
	    childParticleSystem = GetComponentInChildren<ParticleSystem>();
	    audioSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    void OnMouseEnter()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
        childParticleSystem.Play();
        rain.GetComponent<ParticleSystemRenderer>().mesh = Mesh;
    }
    void OnMouseExit()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
        childParticleSystem.Stop();
    }
}
