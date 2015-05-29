using UnityEngine;
using System.Collections;

public class MenuFlotingBox : MonoBehaviour {

    [SerializeField]
    float DelayOffset;
    [SerializeField]
    float Magnitute;
    [SerializeField]
    Vector3 offSet;
    [SerializeField]
    float delay = 0;

    Vector3 orgPosition;
    Vector3 targetPosition;

    bool moveingToTarget;
	// Use this for initialization
	void Start () {
        orgPosition = transform.position;
        delay = Random.insideUnitSphere.magnitude * DelayOffset;
        targetPosition = orgPosition + offSet * Random.Range(-100,100) * 0.02f;
	}
	
	// Update is called once per frame
	void Update () {
	    if(Time.time > delay){
            if(transform.position == orgPosition){
                moveingToTarget = true;
            }
            if(transform.position == targetPosition){
                moveingToTarget = false;
            }

            if (moveingToTarget)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, Magnitute);
            }else{
                transform.position = Vector3.MoveTowards(transform.position, orgPosition, Magnitute);
            }
        }


	}
}
