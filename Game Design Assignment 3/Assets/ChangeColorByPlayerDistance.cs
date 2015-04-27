using System.Runtime.Serialization.Formatters;
using UnityEngine;
using System.Collections;

public class ChangeColorByPlayerDistance : MonoBehaviour
{
    [SerializeField] private float MaxDistance;
    [SerializeField] private float MinDistance;
    private Transform Player;
    private Renderer renderer;
    
	// Use this for initialization
	void Start ()
	{
	    Player = GameObject.FindGameObjectWithTag("Player").transform;
	    renderer = GetComponent<Renderer>();
	}
	
	// Update is called once per frame
	void Update ()
	{
	    var orgDistance = Vector3.Distance(this.transform.position, Player.position);
	    float colorDistance = 0;
	    if (orgDistance > MaxDistance)
	    {
	        colorDistance = 1;
	    }
	    else if (orgDistance < MinDistance)
	    {
	        colorDistance = 0;
	    }
	    else
	    {
	        colorDistance = (orgDistance - MinDistance)/(MaxDistance - MinDistance);
	    }
        Color color  = new Color(1-colorDistance,1-colorDistance,1-colorDistance, 1-colorDistance);
	    renderer.material.color = color;
	}
}
