using UnityEngine;

public class MovableGridCell: GridCell
{
    public float MoveSpeed = -50f;
    protected Grid Grid;
    protected Vector3 TargetPosition;
	// Use this for initialization
    protected bool IsMoved = false;
	protected void Start ()
	{
	    Grid = FindObjectOfType<Grid>();

	}
	
	// Update is called once per frame
	protected void Update ()
	{
        SmoothMove();
	}



    public bool MoveLeft()
    {
        var newPosition = Grid.MoveLeft(this);
        return IsPositionMoved(newPosition);
    }

    public bool MoveRight()
    {
        var newPosition = Grid.MoveRight(this);
        return IsPositionMoved(newPosition);
    }

    public bool MoveUp()
    {
        var newPosition = Grid.MoveUp(this);
        return IsPositionMoved(newPosition);
    }

    public bool MoveDown()
    {
        var newPosition = Grid.MoveDown(this);
        return IsPositionMoved(newPosition);
    }
    protected void SmoothMove()
    {
        if (this.transform.position == TargetPosition)
        {
            IsMoved = false;
        }
        if (IsMoved)
        {
            transform.position = Vector3.MoveTowards(transform.position, TargetPosition, MoveSpeed * Time.deltaTime);
        }
    }

    protected bool IsPositionMoved(Vector3 newPosition)
    {
        if (this.transform.position == newPosition)
        {
            return false;
        }
        TargetPosition = newPosition;
        IsMoved = true;
        return true;
    }
}
