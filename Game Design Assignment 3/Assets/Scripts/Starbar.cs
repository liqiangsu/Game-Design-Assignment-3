using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Starbar : MonoBehaviour
{

    private Image image;
	// Use this for initialization
	void Start ()
	{
	    image = GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update ()
	{
	    image.fillAmount = CollectionManager.MagicCount/CollectionManager.MagicMax;
	}

}
