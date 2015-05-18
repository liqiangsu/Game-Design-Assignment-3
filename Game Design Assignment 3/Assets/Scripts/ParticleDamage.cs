using UnityEngine;
using System.Collections;

public class ParticleDamage : MonoBehaviour
{

    private SaveHelper saveHelper;
	// Use this for initialization
	void Start ()
	{
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("DamagingParticle"))
        {
            saveHelper = FindObjectOfType<SaveHelper>();
            BossLevel1 boss = FindObjectOfType<BossLevel1>();
            boss.IsPlayerInArea = false;
            saveHelper.Load();
        }
    }
}
