using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    // PUBLIC INIT
    public float amount;
    public float length;

    public Camera mainCam;

    // PRIVATE INIT
    float shakeAmount;

    void Awake()
    {
        if (mainCam == null)
        {
            mainCam = Camera.main;
        }
    }

    public void Shake()
    {
        shakeAmount = amount;
        InvokeRepeating("DoShake", 0, 0.01f);
        Invoke("StopShake", length);
    }

    public void SmallShake()
    {
        shakeAmount = amount / 2;
        InvokeRepeating("DoShake", 0, 0.01f);
        Invoke("StopShake", length / 3);
    }

    void DoShake()
    {
        if (shakeAmount > 0)
        {
            Vector3 camPosition = mainCam.transform.position;

            // obtain random x and y offset to shake
            float offsetX = Random.value * shakeAmount * 2 - shakeAmount;
            float offsetY = Random.value * shakeAmount * 2 - shakeAmount;

            camPosition.x += offsetX;
            camPosition.y += offsetY;

            mainCam.transform.position = camPosition;
        }
    }

    void StopShake()
    {
        CancelInvoke("DoShake");
        mainCam.transform.position = new Vector3(0f, 0f, -10f);
    }
}
