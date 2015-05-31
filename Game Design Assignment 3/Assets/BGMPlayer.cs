using System.Linq;
using System.Runtime.Remoting.Lifetime;
using UnityEngine;
using System.Collections;

public class BGMPlayer : MonoBehaviour {

    [SerializeField]
    AudioClip [] Clips;
    AudioSource BGMSrouce;

    private AudioClip org;
    private int currentClipID;
    void Start()
    {
        BGMSrouce = GameObject.FindGameObjectWithTag("BGMSource").GetComponent<AudioSource>();

        BGMSrouce.clip = Clips[GetNextClipID()];
        BGMSrouce.Play();
        Invoke("PlayNextTrack", BGMSrouce.clip.length);
    }

    void Update()
    {
        if(Clips.Length == 0)
        {
            return;
        }
        if (Input.GetButtonUp("Next"))
        {
            PlayClip(GetNextClipID());
        }
        if (Input.GetButtonUp("Pre"))
        {
            PlayClip(GetLastClipID());
        }
        Debug.Log(currentClipID);
    }

    int GetNextClipID()
    {
		if (Clips.Length != 0) {
			currentClipID = (currentClipID + 1) % Clips.Length;
		}
        return currentClipID;
    }

    int GetLastClipID()
    {
		if (Clips.Length != 0) {
			currentClipID = (currentClipID - 1) % Clips.Length;
		}
        return currentClipID;
    }
    void PlayClip(int i)
    {
        BGMSrouce.clip = Clips[i];
        BGMSrouce.Play();
    }

    void PlayNextTrack()
    {
        PlayClip(GetNextClipID());
    }
}
