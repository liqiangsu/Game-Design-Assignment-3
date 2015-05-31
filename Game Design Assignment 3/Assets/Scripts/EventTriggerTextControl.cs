using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EventTriggerTextControl : MonoBehaviour
{

    private GameObject CanvasGameObject;
    private bool isActivate = false;
    private Canvas CanvasCanvas;
    private CanvasRenderer canvasRenderer;
    private CanvasRenderer textCanvsRenderer;

    private float currentTimeOff;
    private float lastActivateTime;
	// Use this for initiali zation
	void Start ()
	{
	    CanvasGameObject = transform.parent.gameObject;
        //CanvasCanvas = CanvasGameObject.GetComponent<Canvas>();
	    canvasRenderer = CanvasGameObject.GetComponent<CanvasRenderer>();
	    textCanvsRenderer = GetComponent<CanvasRenderer>();
        canvasRenderer.SetAlpha(0);
        textCanvsRenderer.SetAlpha(0);
	}
	
	// Update is called once per frame
	void Update () {
	    if (isActivate && canvasRenderer.GetAlpha() < 1)
	    {
            canvasRenderer.SetAlpha(Mathf.Lerp(canvasRenderer.GetAlpha(), 1, 0.2f));
            textCanvsRenderer.SetAlpha(Mathf.Lerp(canvasRenderer.GetAlpha(), 1, 0.2f));
	    }
        if (!isActivate && canvasRenderer.GetAlpha() > 0)
	    {
            canvasRenderer.SetAlpha(Mathf.Lerp(canvasRenderer.GetAlpha(),0,0.1f));
            textCanvsRenderer.SetAlpha(Mathf.Lerp(canvasRenderer.GetAlpha(), 0, 0.1f));
	        //CanvasCanvas.enabled = false;
	    }
	    if (isActivate && Time.time - currentTimeOff > lastActivateTime)
	    {
	        TimeOut();
	    }
	}

    public void Activate(string message,
        float timeOff = 5)
    {
        isActivate = true;
        //CanvasCanvas.enabled = true;
        this.GetComponent<Text>().text = message;
        lastActivateTime = Time.time;
        currentTimeOff = timeOff;
    }

    void TimeOut()
    {
        isActivate = false;
    }
}
