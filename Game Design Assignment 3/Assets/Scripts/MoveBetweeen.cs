using System;
using System.Threading;
using UnityEngine;
using System.Collections;

public class MoveBetweeen : MonoBehaviour
{

    [SerializeField]
    public Vector3 EndPositiosn;
    public Vector3 BeginPosition;
    [SerializeField]
    public float TimeToFinsih;

    public float Delay;


    IEnumerator Start()
    {
        yield return new WaitForSeconds(Delay);
        BeginPosition = transform.position;
        EndPositiosn = transform.FindChild("MovingTarget").position;
        while (true)
        {
            yield return StartCoroutine(MoveObject(transform, BeginPosition, EndPositiosn, TimeToFinsih));
            yield return StartCoroutine(MoveObject(transform, EndPositiosn, BeginPosition, TimeToFinsih));
        }
    }

    IEnumerator MoveObject(Transform thisTransform, Vector3 startPos, Vector3 endPos, float time)
    {
        var i = 0.0f;
        var rate = 1.0f / time;
        while (i < 1.0f)
        {
            i += Time.deltaTime * rate;
            thisTransform.position = Vector3.Lerp(startPos, endPos, i);
            yield return null;
        }
    }
}
