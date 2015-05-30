using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;
using UnityEngine.iOS;

public class TimeMachine : MonoBehaviour
{


    private LinkedList<Dictionary<GameObject, SaveHelper.SimpleTransform>> timeStack = new LinkedList<Dictionary<GameObject, SaveHelper.SimpleTransform>>();

    [SerializeField]
    private float timeRecordFrequence = 0.1f;
    [SerializeField]
    private float MaxRecordTime = 60;

    [SerializeField]
    private float timeLoadFrequence = 0.1f;
    [SerializeField]
    private float MagicUsePerSecond = 3;
    [SerializeField]
    private GameObject InvertColorFilter;
    [SerializeField]
    private GameObject starIcon;
    private AudioSource timeRewindAudioSource;
    private int maxListSize;
    private float lastRecoredTime;

    private float lastLoadingTime;

    Transform playerTransform;


    List<SaveHelper.SmoothMoveJob> MoveJobs = new List<SaveHelper.SmoothMoveJob>();
    // Use this for initialization
    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        maxListSize = Mathf.RoundToInt(MaxRecordTime / timeRecordFrequence);
        timeRewindAudioSource = GetComponent<AudioSource>();
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
    void Update()
    {

        if (Input.GetKeyUp(KeyCode.F) || CollectionManager.MagicCount == 0 || timeStack.Count == 0)
        {
            timeRewindAudioSource.Stop();
            PlayerSwitchToNormalForm();
            if (starIcon)
            {
                starIcon.GetComponent<Animator>().Play("idel");
            }
            if (InvertColorFilter)
            {
                InvertColorFilter.SetActive(false);
            }
        }

        if (Input.GetKey(KeyCode.F))
        {
            if (CollectionManager.MagicCount > 0)
            {
                PlayerSwitchToTimeForm();
                if (!timeRewindAudioSource.isPlaying)
                {
                    timeRewindAudioSource.Play();
                }
                if (starIcon)
                {
                    starIcon.GetComponent<Animator>().Play("rotate");
                }
                if (InvertColorFilter)
                {
                    InvertColorFilter.SetActive(true);
                }
                CollectionManager.UseMagic(MagicUsePerSecond*Time.deltaTime);
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
        else
        {
            if (Time.time - lastRecoredTime > timeRecordFrequence)
            {
                if (timeStack.Count > maxListSize)
                {
                    timeStack.RemoveFirst();
                }

                //avoid checking when just stated
                //check if player moved
                if (timeStack.Last == null || timeStack.Last.Value[playerTransform.gameObject].Position != playerTransform.position)
                {
                    timeStack.AddLast(SaveHelper.RecordGameObjectTransform());
                    lastRecoredTime = Time.deltaTime;
                }
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
                    var cube = go.GetComponent<Cube>();
                    if (cube)
                    {
                        cube.IsMoved = false;
                    }
                    go.transform.position = savedTransform.Position;
                    go.transform.rotation = savedTransform.Quaternation;
                    go.transform.localScale = savedTransform.Scale;
                    go.transform.localPosition = savedTransform.LocalPosition;
                }
            }
        }
    }

    void PlayerSwitchToTimeForm()
    {
        for (int i = 0; i < playerTransform.childCount; i++)
        {
            playerTransform.GetChild(i).gameObject.SetActive(false);

        }
        playerTransform.FindChild("TimeForm").gameObject.SetActive(true);
    }

    void PlayerSwitchToNormalForm()
    {
        for (int i = 0; i < playerTransform.childCount; i++)
        {
            playerTransform.GetChild(i).gameObject.SetActive(true);

        }
        playerTransform.FindChild("TimeForm").gameObject.SetActive(false);
    }
}
