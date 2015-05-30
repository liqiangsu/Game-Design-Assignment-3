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
    private float movingTime = 0f;
    [SerializeField]
    private float RemovingSpeed = 0.3f;
    [SerializeField]
    private string[] triggeringTags = { "player" };

    [SerializeField] private Material triggedMaterial = null;

    public bool IsTriggered = false;
    // Use this for initialization
    protected void Start()
    {
        if (targetPosition == Vector3.zero)
        {
            targetPosition = RemovingObject.transform.position + new Vector3(0, 20, 0);
        }
    }

    // Update is called once per frame
    protected void FixedUpdate()
    {
        if (IsTriggered && IsEnabled)
        {
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
        if (IsTriggered)
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
                IsTriggered = false;
            }
        }
    }


    protected void OnTriggerEnter(Collider other)
    {
        if (triggeringTags.Contains(other.gameObject.tag))
        {
            if (IsEnabled) { 
                AudioSource.PlayClipAtPoint(GetComponent<AudioSource>().clip, transform.position, 1f);
                if (triggedMaterial)
                {
                    GetComponent<Renderer>().material = triggedMaterial;
                }
            }
            IsTriggered = true;
        }
    }

    void DestoryObject()
    {
        RemovingObject.transform.position = Extesions.BlackHole;
    }
}
