using System.IO;
using UnityEngine;
using System.Collections;
using UnityEditor;
using Object = UnityEngine.Object;
public class MapLoad : MonoBehaviour {


    [MenuItem("Tools/Group10/LoadMap")]
    static void LoadMap()
    {
        Grid grid = FindObjectOfType<Grid>();

        string levelName = Path.GetFileNameWithoutExtension(EditorApplication.currentScene);
        
        grid.ReadLevel(levelName);
    }

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
