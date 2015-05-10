using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour {
    private Vector3 originPosition;
    private Quaternion originRotation;
    public float InitalShakeDecay;
    public float InitalShakeIntensity;
    private float shakeDecay;
    private float shakeIntensity;

    void Update()
    {
        if (shakeIntensity > 0)
        {
            transform.position = originPosition + Random.insideUnitSphere * shakeIntensity;
            transform.rotation = new Quaternion(
            originRotation.x + Random.Range(-shakeIntensity, shakeIntensity) * .2f,
            originRotation.y + Random.Range(-shakeIntensity, shakeIntensity) * .2f,
            originRotation.z + Random.Range(-shakeIntensity, shakeIntensity) * .2f,
            originRotation.w + Random.Range(-shakeIntensity, shakeIntensity) * .2f);
            shakeIntensity -= shakeDecay;
        }
    }

    public void Shake()
    {
        originPosition = transform.position;
        originRotation = transform.rotation;
        shakeIntensity = InitalShakeIntensity;
        shakeDecay = InitalShakeDecay;
    }
}
