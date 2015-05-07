using UnityEngine;
using System.Collections;

public class ResetTrigger: MonoBehaviour
{
    private SaveHelper save;

    void Start()
    {
        save = GameObject.Find("SavePoint").GetComponent<SaveHelper>();
    }
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            save.Load();
            //Application.LoadLevel("Level1");
        }
    }
	void OnTriggerEnter(Collider other){
		if (other.CompareTag ("Player")) {
			save.Load();
            //Application.LoadLevel("Level1");
		}
	}
}
