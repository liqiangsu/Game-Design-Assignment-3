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



    List<SaveHelper.SmoothMoveJob> MoveJobs = new List<SaveHelper.SmoothMoveJob>();
	// Use this for initialization
	void Start ()
	{
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
                        if (theRigibody.position != o.End)
                        {
                            Debug.Log("rher");
                        }
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
	    if (!Input.GetKey(KeyCode.F) && Input.GetButtonDown("Push") && Time.time - lastRecoredTime > timeRecordFrequence)
	    {

	    }

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
	}

    public void SnapTime()
    {
        timeStack.AddLast(SaveHelper.RecordGameObjectPositions());

        //limiting the size of the stack
        if (timeStack.Count > lastRecoredTime)
        {
            timeStack.RemoveFirst();
        }
        lastRecoredTime = Time.time;
    }
    public void Load(Dictionary<GameObject, SaveHelper.SimpleTransform> lastSave)
    {
        var playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        if (lastSave != null)
        {
            foreach (var entry in lastSave)
            {
                if (entry.Key != null)
                {
                    //directly move player objects will cause bug, this is a walk arround
                    if (entry.Key.transform.IsChildOf(playerTransform) && !entry.Key.transform.CompareTag("Player"))
                    {
                        continue;
                    }
                    entry.Key.transform.rotation = entry.Value.Quaternation;

                    //disable collider to prepare for strigh movement back to saved position.

                    var theRigibody = entry.Key.GetComponent<Rigidbody>();

                    if (theRigibody)
                    {
                        theRigibody.detectCollisions = false;
                        //theRigibody.position = entry.Value.Position;
                    }


                    MoveJobs.Add(new SaveHelper.SmoothMoveJob() { GameObject = entry.Key, Start = entry.Key.transform.position, End = entry.Value.Position, StartTime = Time.time });
                    entry.Key.transform.localScale = entry.Value.Scale;
                }
            }
        }
    }
}
