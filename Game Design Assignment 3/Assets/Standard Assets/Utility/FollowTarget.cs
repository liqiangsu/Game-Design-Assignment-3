using System;
using System.Collections;
using System.ComponentModel;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{

    // The target we are following
    [SerializeField]
    public Transform Target;
    // The distance in the x-z plane to the target
    [SerializeField] public Vector3 Offset = new Vector3(0, 3, -3);
    [SerializeField]
    public float transitionDuration = 2.5f;
    private Vector3 velocity = Vector3.zero;

    // Use this for initialization
    void Start() { }

    // Update is called once per frame
    void LateUpdate()
    {
        // Early out if we don't have a target
        if (!Target)
            return;
        velocity = GetComponent<Camera>().velocity;


        StartCoroutine(Transition());
        transform.LookAt(Target);
    }

    private IEnumerator Transition()
    {
        float t = 0.0f;
        Vector3 startingPos = transform.position;
        while (t < 1.0f)
        {
            t += Time.deltaTime*(Time.timeScale/transitionDuration);

            transform.position = Vector3.Lerp(startingPos, Target.position + Offset, t);
            yield return 0;
        }
    }
}