using UnityEngine;

public class MovableCell: MonoBehaviour, ICell
{
    public int GridX { get; set; }
    public int GridY { get; set; }

    public GameObject GameObject
    {
        get { return gameObject; }
    }

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
	    GetKeyInput();
        SmoothMove();
	    if (Debug.isDebugBuild)
	    {
	        TextMesh tm = GetComponentInChildren<TextMesh>();
	        tm.text = GridX + " , " + GridY;
	    }
	}

    private void GetKeyInput()
    {
        //if object is moving , ignore input
        if (IsMoved)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            MoveLeft();
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            MoveRight();
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            MoveUp();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            MoveDown();
        }
    }

    protected bool MoveLeft()
    {
        var newPosition = Grid.MoveLeft(this);
        return IsPositionMoved(newPosition);
    }

    protected bool MoveRight()
    {
        var newPosition = Grid.MoveRight(this);
        return IsPositionMoved(newPosition);
    }

    protected bool MoveUp()
    {
        var newPosition = Grid.MoveUp(this);
        return IsPositionMoved(newPosition);
    }

    protected bool MoveDown()
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
