using UnityEngine;

public class PushableCell : MovableCell, IPushableCell {
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

    public new bool MoveLeft()
    {
        return base.MoveLeft();
    }

    public new bool MoveRight()
    {
        return base.MoveRight();
    }

    public new bool MoveUp()
    {
        return base.MoveUp();
    }

    public new bool MoveDown()
    {
        return base.MoveDown();
    }
}
