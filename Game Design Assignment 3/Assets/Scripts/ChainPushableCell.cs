using UnityEngine;

public class ChainPushableCell : MovableCell {
    // Use this for initialization
	void Start () {
	    base.Start();
	}
	
	// Update is called once per frame
	void Update () {
        SmoothMove();
        if (Debug.isDebugBuild)
        {
            TextMesh tm = GetComponentInChildren<TextMesh>();
            tm.text = GridX + " , " + GridY;
        }
	}
}
