using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmitterScript : MonoBehaviour
{    
    [SerializeField]
    GameObject redBlink;    // child of the emitter to indicate endpoint of the laser

    // parameters of the ray
    [SerializeField]
    float rayWidth;         // ray width
    [SerializeField]
    float rayLength;        // ray length

    // layers masks to detect certain objects
    [SerializeField]
    LayerMask obstacleMask;
    [SerializeField]
    LayerMask sunkenMask;
    [SerializeField]
    LayerMask playerMask;

    // references to objects
    BlinkManager bm;
    LineRenderer rayLineRenderer;
    PlayerController thePlayer;

    Vector3 endCoord;   // end coordinate of the line renderer


    // Start is called before the first frame update
    void Start()
    {
        bm = FindObjectOfType<BlinkManager>();
        thePlayer = FindObjectOfType<PlayerController>();

        // setting the line renderer
        rayLineRenderer = GetComponent<LineRenderer>();
        Vector3[] initRayPositions = new Vector3[2] { Vector3.zero, Vector3.zero };
        rayLineRenderer.SetPositions(initRayPositions);
        rayLineRenderer.SetWidth(rayWidth, rayWidth);   
    }

    // Update is called once per frame
    void Update()
    {
        // crate vector that we will allign our ray to
        Vector3 upVec = transform.TransformDirection(Vector3.up);

        //Debug.DrawRay(transform.position, upVec * rayLength, Color.red);

        endCoord = Raycast(upVec);

        rayLineRenderer.SetPosition(0, transform.position);
        rayLineRenderer.SetPosition(1, endCoord);
    }

    Vector3 Raycast(Vector3 vector)
    {
        RaycastHit hitInfo1; // hit for obstacles
        RaycastHit hitInfo2; // hot for the player and sunkens

        // check player hit
        if (Physics.Raycast(transform.position, vector, out hitInfo2, rayLength, playerMask))
        {
            float dstToTarget = Vector3.Distance(transform.position, hitInfo2.point);

            if (!Physics.Raycast(transform.position, vector, dstToTarget, sunkenMask) 
                && !Physics.Raycast(transform.position, vector, dstToTarget, obstacleMask)) // Если на пути нет обломков
            {
                //Debug.Log(gameObject.name + " killed the player!");
                thePlayer.DestroyPlayer();
                SetRedBlinkPosition(hitInfo2.point);
                return hitInfo2.point;

            }
        }

        // check obstacle and sunken hit
        if (Physics.Raycast(transform.position, vector, out hitInfo1, rayLength, obstacleMask))
        {  
            // if hit sunken
            if (Physics.Raycast(transform.position, vector, out hitInfo2, rayLength, sunkenMask))
            {
                float dstToTarget = Vector3.Distance(transform.position, hitInfo2.point);
                if (!Physics.Raycast(transform.position, vector, dstToTarget, obstacleMask)) // Если на пути нет препятствий
                {
                    hitInfo2.transform.gameObject.GetComponent<EnemyController>().CreateBlink();
                    SetRedBlinkPosition(hitInfo2.point);
                    return hitInfo2.point;
                }
            }

            SetRedBlinkPosition(hitInfo1.point);
            return hitInfo1.point;
        }

        return (transform.position);
    }

    void SetRedBlinkPosition(Vector3 pos)
    {
        redBlink.transform.position = pos;
    }
}
