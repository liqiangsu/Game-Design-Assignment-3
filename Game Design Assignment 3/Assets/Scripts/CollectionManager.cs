using UnityEngine;
using System.Collections;

public class CollectionManager : MonoBehaviour
{



    [SerializeField] public static int MagicCount;

    void OnGUI()
    {
        GUI.TextArea(new Rect(20, 20, 50, 20), "" + MagicCount);
    }
	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	    
	}
}
