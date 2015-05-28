using UnityEngine;
using System.Collections;

public class CollectionManager : MonoBehaviour
{



    [SerializeField] public static float MagicCount;
    [SerializeField] public static float MagicMax = 25;
    void OnGUI()
    {
#if DEBUG
        GUI.TextArea(new Rect(20, 20, 50, 20), "" + MagicCount);
#endif
    }
	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    public static void CollectMagic(float amount)
    {
        MagicCount += amount;
        if (MagicCount > MagicMax)
        {
            MagicCount = MagicMax;
        }
    }

    public static void UseMagic(float amount)
    {
        MagicCount -= amount;
        if (MagicCount < 0)
        {
            MagicCount = 0;
        }
    }
}
