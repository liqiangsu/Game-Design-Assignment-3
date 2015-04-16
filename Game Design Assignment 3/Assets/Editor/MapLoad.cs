using System.IO;
using UnityEngine;
using System.Collections;
using UnityEditor;
using Object = UnityEngine.Object;
public class MapLoad : MonoBehaviour
{

    private static Grid grid;
    [MenuItem("Tools/Group10/LoadMap")]
    static void LoadMap()
    {
        grid = FindObjectOfType<Grid>();

        string levelName = Path.GetFileNameWithoutExtension(EditorApplication.currentScene);
        
        grid.ReadLevel(levelName);
    }

    [MenuItem("Tools/Group10/SaveMap")]
    static void SaveMap()
    {
        if (grid == null)
        {
            grid = FindObjectOfType<Grid>();
        }
        if (grid.gridCells == null)
        {
            grid.Load();
        }
        //string levelName = Path.GetFileNameWithoutExtension(EditorApplication.currentScene);
        string levelName = "test";
        string path = "Assets/Resources/" + levelName + ".txt";
        StreamWriter writer = File.CreateText(path);
        for (int i = 0; i < grid.gridCells.GetLength(0); i++)
        {
            for (int j = 0; j < grid.gridCells.GetLength(1); j++)
            {
                writer.Write('E');
            }
            writer.Write(System.Environment.NewLine);
        }
        writer.Close();
    }
	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}