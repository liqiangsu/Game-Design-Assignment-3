using System.Linq;
using UnityEngine;
using System.Collections;

public class RemoveTrgger : MonoBehaviour
{
    [SerializeField]
    GameObject RemovingObject;
    [SerializeField]
    private Vector3 targetPosition;
    [SerializeField]
    bool IsEnabled = true;
    [SerializeField]
    bool IsDestoryAfterMovment = false;
    [SerializeField]
    private float movingTime = 1f;
    [SerializeField]
    private float RemovingSpeed = 0.3f;
    [SerializeField]
    private string[] triggeringTags = { "player" };
    private bool isTriggered = false;
    // Use this for initialization
    protected void Start()
    {
        if (targetPosition == Vector3.zero)
        {
            targetPosition = RemovingObject.transform.position + new Vector3(0, 20, 0);
        }
    }

    // Update is called once per frame
    protected void Update()
    {
        if (isTriggered && IsEnabled)
        {
            //just removing objects' collider, so it fall
            //TODO makeit slide to a position
            var rigi = RemovingObject.GetComponent<Rigidbody>();
            var col = RemovingObject.GetComponent<Collider>();
            col.enabled = false;
            rigi.useGravity = false;
            if (IsDestoryAfterMovment)
            {
                Invoke("DestoryObject", movingTime);
            }

            //disable this trigger afterit is been activated
            IsEnabled = false;
        }
        if (isTriggered)
        {
            if (RemovingObject.transform.position != targetPosition)
            {
                RemovingObject.transform.position =
                    Vector3.MoveTowards(RemovingObject.transform.position, targetPosition,
                        RemovingSpeed);
            }
            else
            {
                var rigi = RemovingObject.GetComponent<Rigidbody>();
                rigi.constraints = RigidbodyConstraints.FreezeAll;
                var col = RemovingObject.GetComponent<Collider>();
                col.enabled = true;
                isTriggered = false;
            }
        }
    }


    protected void OnTriggerEnter(Collider other)
    {
        if (triggeringTags.Contains(other.gameObject.tag))
        {
            isTriggered = true;
        }
    }

    void DestoryObject()
    {
        RemovingObject.transform.position = Extesions.BlackHole;
    }
}
