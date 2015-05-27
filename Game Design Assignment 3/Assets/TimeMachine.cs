using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;

public class TimeMachine : MonoBehaviour
{


    private LinkedList<Dictionary<GameObject, SaveHelper.SimpleTransform>> timeStack = new LinkedList<Dictionary<GameObject, SaveHelper.SimpleTransform>>();

    [SerializeField] private float timeRecordFrequence = 0.1f;
    [SerializeField] private float MaxRecordTime = 60;

    [SerializeField] private float timeLoadFrequence = 0.1f;
    private int maxListSize;
    private float lastRecoredTime;

    private float lastLoadingTime;
    
    Transform playerTransform;


    List<SaveHelper.SmoothMoveJob> MoveJobs = new List<SaveHelper.SmoothMoveJob>();
	// Use this for initialization
	void Start ()
	{
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
	    maxListSize = Mathf.RoundToInt(MaxRecordTime/timeRecordFrequence);
	}

    void FixedUpdate()
    {
        foreach (var job in MoveJobs)
        {
            var timeSicneStart = Time.time - job.StartTime;
            var percentage = timeSicneStart * job.Speed;
            job.GameObject.transform.position = Vector3.Lerp(job.Start, job.End, percentage);
        }
        if (MoveJobs.Count > 0)
        {
            //when back to saved position, re-enable collider
            MoveJobs.Where(o => o.GameObject.transform.position == o.End).ToList().ForEach(
                (o) =>
                {
                    var theRigibody = o.GameObject.GetComponent<Rigidbody>();
                    if (theRigibody)
                    {
                        //reset collision detection and velocity;
                        theRigibody.detectCollisions = true;
                        theRigibody.velocity = Vector3.zero;
                    }

                }
                );
            MoveJobs.RemoveAll(o => o.GameObject.transform.position == o.End);
        }
    }


	// Update is called once per frame
	void Update () {


        if(Input.GetKey(KeyCode.F))
	    {
            Debug.Log(timeStack.Count);
            if (Time.time - lastLoadingTime > timeLoadFrequence)
            {
                if (timeStack.Count > 0)
                {
                    var snapShotDictionary = timeStack.Last.Value;
                    Load(snapShotDictionary);
                    timeStack.RemoveLast();
                }
                lastLoadingTime = Time.time;
            }
        }
        else
        {
            if (Time.time - lastRecoredTime > timeRecordFrequence)
            {
                if (timeStack.Count > maxListSize)
                {
                    timeStack.RemoveFirst();
                }
                timeStack.AddLast(SaveHelper.RecordGameObjectTransform());
                lastRecoredTime = Time.deltaTime;
            }
        }
	}

    public void SnapTime()
    {
        timeStack.AddLast(SaveHelper.RecordGameObjectTransform());

        //limiting the size of the stack
        if (timeStack.Count > lastRecoredTime)
        {
            timeStack.RemoveFirst();
        }
        lastRecoredTime = Time.time;
    }
    public void Load(Dictionary<GameObject, SaveHelper.SimpleTransform> snapShot)
    {
        if (snapShot != null)
        {
            foreach (var entry in snapShot)
            {
                if (entry.Key != null)
                {
                    var go = entry.Key;
                    var savedTransform = entry.Value;
                    if (go.transform.IsChildOf(playerTransform) && !go.transform.CompareTag("Player"))
                    {
                        continue;
                    }
                    go.transform.position = savedTransform.Position;
                    go.transform.rotation = savedTransform.Quaternation;
                    go.transform.localScale = savedTransform.Scale;
                    go.transform.localPosition = savedTransform.LocalPosition;
                }
            }
        }
    }
}
