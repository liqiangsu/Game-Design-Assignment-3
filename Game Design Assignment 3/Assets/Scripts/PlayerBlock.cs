using UnityEngine;
using System.Collections;

public class PlayerBlock : MovableGridCell {

    protected void Start()
    {
        base.Start();
    }

    protected void Update()
    {
        GetKeyInput();
        base.Update();
    }
    private void GetKeyInput()
    {
        //if object is moving , ignore input
        if (IsMoved)
        {
            return;
        }
        if (Input.GetAxisRaw("Horizontal") == -1)
        {
            MoveLeft();
        }
        if (Input.GetAxisRaw("Horizontal") == 1)
        {
            MoveRight();
        }
        if (Input.GetAxisRaw("Vertical") == 1)
        {
            MoveUp();
        }
        if (Input.GetAxisRaw("Vertical") == -1)
        {
            MoveDown();
        }
    }
}
