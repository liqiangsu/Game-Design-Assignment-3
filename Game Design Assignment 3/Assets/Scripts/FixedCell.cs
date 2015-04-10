using UnityEngine;

public class FixedCell : MonoBehaviour, ICell {
    public int GridX { get; set; }
    public int GridY { get; set; }

    public GameObject GameObject
    {
        get
        {
            return gameObject;
        }
    }

    // Use this for initialization
	void Start ()
	{
	}
	
	// Update is called once per frame
	void Update () {
	
	}

}
