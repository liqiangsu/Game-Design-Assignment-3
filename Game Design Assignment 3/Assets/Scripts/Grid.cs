using System;
using System.Linq;
using UnityEngine;
public class Grid : MonoBehaviour
{
    private int Width;
    private int Height;
    public float CellSize;
    public float CellSpacing = 0.1f;
    public GameObject FixedCell;
    public GameObject MoveableCell;
    public GameObject EmptyCell;
    public GameObject ChainPushableCell;
    public GameObject ExitCell;
	public GameObject ResetCel;
    public GameObject PlayerCell;
    public GameObject CellParent;

    /// use to group the cells
    private static int CellId = 0;
    public GridCell[,] gridCells;

    // Use this for initialization
	void Start ()
	{
        //ReadLevel("Level1");
        Load();
	}

    private void Load()
    {
        GridCell[] gameObjects = FindObjectsOfType<GridCell>();

        int maxX = gameObjects.Max(o => o.GridX);
        int maxY = gameObjects.Max(o => o.GridY);
        gridCells = new GridCell[maxX + 1,maxY +1];
        foreach (GridCell cell in gameObjects)
        {
            gridCells[cell.GridX, cell.GridY] = cell;
        }
    }

    public void ReadLevel(string levelName)
    {
        TextAsset level = Resources.Load(levelName) as TextAsset;
        if (!level)
        {
            throw new Exception("Level " + levelName +  " file not Found");
        }
        if (CellParent == null)
        {
            CellParent = new GameObject(){name = "Boxes"};
        }

        var lines = level.text.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
        Height = lines.Length;
        Width = lines[0].Length;
        char[,] mapInChar = new char[Width,Height];
        for(int line = 0; line < Height; line++)
        {
            var l = lines[line];
            if (l.Length != Width)
            {
                throw new Exception("Invalid map, Length Must be constance");
            }
            for (int c = 0; c < Width; c++)
            {
                char ch = l[c];
                mapInChar[c, line] = ch;
            }
        }
        InstanceGrid(mapInChar);
    }

    private void InstanceGrid(char[,] mapData)
    {
        gridCells = new GridCell[Width,Height];
        for (int i = 0; i < gridCells.GetLength(0); i++)
        {
            for (int j = 0; j < gridCells.GetLength(1); j++)
            {
                switch (mapData[i,j])
                {
                    case 'M':
                        gridCells[i, j] = InitalCell(MoveableCell, i, j);
                        break;
                    case 'F':
                        gridCells[i, j] = InitalCell(FixedCell, i, j);
                        break;
                    case 'C':
                        gridCells[i, j] = InitalCell(ChainPushableCell, i, j);
                        break;
                    case 'P':
                        gridCells[i, j] = InitalCell(PlayerCell, i, j);
                        break;
                    case 'X':
                        gridCells[i, j] = InitalCell(ExitCell, i, j);
                        break;
				    case 'R':
					    gridCells[i, j] = InitalCell(ResetCel, i, j);
					    break;
				default:
					gridCells[i, j] = InitalCell(EmptyCell, i, j);
                        break;
                }

                
           }
        }
    }


    private GridCell InitalCell(GameObject gameObject, int x, int y)
    {
        GameObject instance = Instantiate(gameObject);
        instance.name += "_" + CellId++;
        GridCell gridCell = instance.GetComponent<GridCell>();
        if (gridCell == null)
        {
            throw new Exception("Cell Create Faild, Objective do not have ICell component");
        }
        gridCell.GridX = x;
        gridCell.GridY = y;
        instance.transform.position = GetPosition(x, y);
        instance.transform.SetParent(CellParent.transform);

        return gridCell;
    }
	// Update is called once per frame
	void Update () {
	}

    private Vector3 GetPosition(int x, int y)
    {
        return new Vector3(x * CellSize + x * CellSpacing ,0, y * -CellSize - y * CellSpacing);
    }
    public Vector3 MoveLeft(GridCell aGridCell)
    {
        if (aGridCell.GridX <= 0)
        {
            //if not moved return same position
            return aGridCell.gameObject.transform.position;
        }
        GridCell bGridCell = gridCells[aGridCell.GridX -1, aGridCell.GridY];

        return Move(aGridCell, bGridCell);
    }
    
    public Vector3 MoveRight(GridCell aGridCell)
    {
        if (aGridCell.GridX >= gridCells.GetLength(0) - 1)
        {
            //if not moved return same position
            return aGridCell.gameObject.transform.position;
        }
        GridCell bGridCell = gridCells[aGridCell.GridX + 1, aGridCell.GridY];
        return Move(aGridCell, bGridCell);
    }

    private Vector3 Move(GridCell aGridCell,
        GridCell bGridCell)
    {
        if (bGridCell is MoveOnceGridCell && !(aGridCell is MoveOnceGridCell))
        {
            var other = bGridCell as MoveOnceGridCell;
            int oldX = aGridCell.GridX;
            int oldY = aGridCell.GridY;
            int newX = bGridCell.GridX;
            int newY = bGridCell.GridY;
            bool isMoved = MoveMoveableCells(other, oldX, oldY, newX, newY);
            if (isMoved)
            {
                Destroy(gridCells[newX, newY].gameObject);
                gridCells[newX, newY] = aGridCell;
                aGridCell.GridX = newX;
                aGridCell.GridY = newY;
                gridCells[oldX, oldY] = InitalCell(EmptyCell, oldX, oldY);
                var targetPosition = GetPosition(newX, newY);
                return targetPosition;
            }
        }
        if (bGridCell is ChainPushableGridCell)
        {
            ChainPushableGridCell pGridCell = bGridCell as ChainPushableGridCell;
            int oldX = aGridCell.GridX;
            int oldY = aGridCell.GridY;
            int newX = bGridCell.GridX;
            int newY = bGridCell.GridY;

            var isMoved = MoveMoveableCells(pGridCell, oldX, oldY, newX, newY);
            if (isMoved) {
                Destroy(gridCells[newX, newY].gameObject);
                gridCells[newX, newY] = aGridCell;
                aGridCell.GridX = newX;
                aGridCell.GridY = newY;
                gridCells[oldX, oldY] = InitalCell(EmptyCell, oldX, oldY);
                var targetPosition = GetPosition(newX, newY);
                return targetPosition;
            }
        }
        if (bGridCell is EmptyGridCell)
        {
            int oldX = aGridCell.GridX;
            int oldY = aGridCell.GridY;
            int newX = bGridCell.GridX;
            int newY = bGridCell.GridY;

            Destroy(bGridCell.gameObject);
            gridCells[newX, newY] = aGridCell;
            aGridCell.GridX = newX;
            aGridCell.GridY = newY;
            gridCells[oldX, oldY] = InitalCell(EmptyCell, oldX, oldY);
            var targetPosition = GetPosition(newX, newY);
            return targetPosition;
        }
        if(bGridCell is ExitGridCell || bGridCell is ResetGridCell)
        {
            int oldX = aGridCell.GridX;
            int oldY = aGridCell.GridY;
            int newX = bGridCell.GridX;
            int newY = bGridCell.GridY;
            gridCells[newX, newY] = aGridCell;
            aGridCell.GridX = newX;
            aGridCell.GridY = newY;
            gridCells[oldX, oldY] = InitalCell(EmptyCell, oldX, oldY);
            var targetPosition = GetPosition(newX, newY);
            return targetPosition;
        }

        return aGridCell.gameObject.transform.position;
    }

    //return true if the cell is acutall moved
    private static bool MoveMoveableCells(MovableGridCell pGridCell,
                                            int oldX,
                                            int oldY,
                                            int newX,
                                            int newY
)
    {
        bool isMoved = false;
        if (newX < oldX)
        {
            isMoved = pGridCell.MoveLeft();
        }
        else if (newX > oldX)
        {
            isMoved = pGridCell.MoveRight();
        }
        if (newY < oldY)
        {
            isMoved = pGridCell.MoveUp();
        }
        else if (newY > oldY)
        {
            isMoved = pGridCell.MoveDown();
        }
        return isMoved;
    }

    public Vector3 MoveUp(GridCell aGridCell)
    {
        if (aGridCell.GridY <= 0)
        {
            //if not moved return same position
            return aGridCell.gameObject.transform.position;
        }
        GridCell bGridCell = gridCells[aGridCell.GridX, aGridCell.GridY -1];
        return Move(aGridCell, bGridCell);
    }

    public Vector3 MoveDown(GridCell aGridCell)
    {
        if (aGridCell.GridY >= gridCells.GetLength(1) - 1)
        {
            //if not moved return same position
            return aGridCell.gameObject.transform.position;
        }
        GridCell bGridCell = gridCells[aGridCell.GridX, aGridCell.GridY + 1];
        return Move(aGridCell, bGridCell);
    }

}
