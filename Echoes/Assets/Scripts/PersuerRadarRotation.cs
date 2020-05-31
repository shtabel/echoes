using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersuerRadarRotation : MonoBehaviour
{
    // PUBLIC INIT
    [Range(0f, 360f)]
    public float rotationDegree;
    public float rayLength;

    public PersuerController persuer;
    public LayerMask playerMask;
    public LayerMask obstacleMask;

    // PRIVATE INIT
    float blinkDelay = 0.4f;
    float nextTimeBlink;

    BlinkManager bm;

    // Start is called before the first frame update
    void Start()
    {
        bm = FindObjectOfType<BlinkManager>();

        nextTimeBlink = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        // rotate ray
        transform.Rotate(0.0f, 0.0f, -rotationDegree * Time.deltaTime);

        Raycast();

        transform.position = persuer.transform.position;
    }

    public void Raycast()
    {
        Vector3 upVec = transform.TransformDirection(Vector3.up);

        // draw ray in scene window
        Debug.DrawRay(transform.position, upVec * rayLength, Color.blue);

        RaycastHit hitInfo;
        if (Physics.Raycast(transform.position, upVec, out hitInfo, rayLength, playerMask) && (Time.time > nextTimeBlink))
        {
            float dstToTarget = Vector3.Distance(transform.position, hitInfo.point);

            if (!Physics.Raycast(transform.position, upVec, dstToTarget, obstacleMask))
            {
                bm.CreateBlinkFollow(bm.blinkCircleRed, transform.position, gameObject);
                bm.CreateBlink(bm.blinkRed, hitInfo.transform.position);

                persuer.ChaseToPosition(hitInfo.transform.position);

                nextTimeBlink = Time.time + blinkDelay;
            }
        }
    }
}
