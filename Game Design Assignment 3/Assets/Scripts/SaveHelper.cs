using System;
using System.IO;
using System.Linq;
using UnityEditor.Sprites;
using UnityEngine;
using System.Collections.Generic;

public class SaveHelper : MonoBehaviour {
    public class SimpleTransform
    {
        public Vector3 Position = Vector3.zero;
        public Quaternion Quaternation = Quaternion.identity;
        public Vector3 Scale = new Vector3(1, 1, 1);
        public Vector3 LocalPosition = Vector3.zero;
    }

    public class SmoothMoveJob
    {
        public GameObject GameObject;
        public Vector3 Start;
        public Vector3 End;
        public float StartTime;
        public float Speed = 2;
    }
    Dictionary<GameObject, SimpleTransform> lastSave;

    List<SmoothMoveJob> MoveJobs = new List<SmoothMoveJob>();
	// Use this for initialization
	void Start () {
	    Save();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            Save();
            Debug.Log("Saved");
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            Load();
            Debug.Log("Loaded");
        }
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
    public void Save()
    {
        lastSave = RecordGameObjectTransform();
    }

    public static Dictionary<GameObject, SimpleTransform> RecordGameObjectTransform()
    {
        var gos = GameObject.FindObjectsOfType<GameObject>();
        Dictionary<GameObject, SimpleTransform> record = new Dictionary<GameObject, SimpleTransform>();
        foreach (GameObject go in gos)
        {
            var rigi = go.GetComponent<Rigidbody>();
            if (rigi && !rigi.isKinematic)
            {
                var transform = new SimpleTransform()
                {
                    Position = go.transform.position,
                    Quaternation = go.transform.rotation,
                    Scale = go.transform.localScale,
                    LocalPosition = go.transform.localPosition
                };
                record.Add(go, transform);
            }
        }
        return record;
    }

    public void Load()
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


                    MoveJobs.Add(new SmoothMoveJob(){GameObject = entry.Key, Start = entry.Key.transform.position, End = entry.Value.Position, StartTime = Time.time});
                    entry.Key.transform.localScale = entry.Value.Scale;
                }
            }
        }
    }

    public void SaveCompletedLevel(string levelName)
    {
        using (
            FileStream fs = File.Open(Application.persistentDataPath + "/level.sav",
                FileMode.Append))
        {
            StreamWriter sw = new StreamWriter(fs);
            sw.WriteLine(levelName);
        }
    }

    public string[] LoadCompletedlevel()
    {
        using (
            FileStream fs = File.Open(Application.persistentDataPath + "/level.sav", FileMode.Open)
            )
        {
            StreamReader sr = new StreamReader(fs);
            List<String> lines = new List<string>();
            while (!sr.EndOfStream)
            {
                sr.ReadLine();
            }
            return lines.ToArray();
        }
    }
}
