using UnityEngine;
using System.Collections;

public class ResetTrigger: MonoBehaviour
{
    private SaveHelper save;

    void Start()
    {
        save = GameObject.FindObjectOfType<SaveHelper>();
    }
    void Update()
    {
    }
	void OnTriggerEnter(Collider other){
		if (other.CompareTag ("Player")) {
			save.Load();
		}
	}
}
