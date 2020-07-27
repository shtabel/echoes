using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointMovement : MonoBehaviour
{
    [SerializeField]
    public float speed;                // speed of the object

    [SerializeField]
    Vector3[] localWaypoints;   // local waypoints (relative to the object)
    Vector3[] globalWaypoints;  // global waypoints (in world's coordinates)

    int fromWaypointIndex;      // index of the global ypoint that we movong awau from
    float percentBtwWaypoints;  // percentage that we moved from one waypoint to another (from 0 to 1)


    // Start is called before the first frame update
    void Start()
    {
        globalWaypoints = SetGlobalWaypoints();
    }

    Vector3[] SetGlobalWaypoints()
    {
        Vector3[] gw = new Vector3[localWaypoints.Length];
        for (int i = 0; i < localWaypoints.Length; i++)
        {
            gw[i] = localWaypoints[i] + transform.position;
        }

        return gw;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 velocity = CalculateMovement();

        transform.Translate(velocity);
    }

    Vector3 CalculateMovement()
    {
        fromWaypointIndex %= globalWaypoints.Length;    // reset it to 0 each time it reaches globalWaypoints.Length
        int toWaypointIndex = (fromWaypointIndex + 1) % globalWaypoints.Length;

        float dstBtwWaypoints = Vector3.Distance(globalWaypoints[fromWaypointIndex], globalWaypoints[toWaypointIndex]);

        percentBtwWaypoints += Time.deltaTime * speed / dstBtwWaypoints;

        Vector3 newPos = Vector3.Lerp(globalWaypoints[fromWaypointIndex], globalWaypoints[toWaypointIndex], percentBtwWaypoints);

        if (percentBtwWaypoints >= 1)
        {
            percentBtwWaypoints = 0;
            fromWaypointIndex++;
        }

        return newPos - transform.position;
    }

    void OnDrawGizmos() 
    {
        if(localWaypoints != null)
        {
            Gizmos.color = Color.red;
            float size = .5f;

            for (int i = 0; i < localWaypoints.Length; i++)
            {
                Vector3 globalWaypointPos = (Application.isPlaying) ? globalWaypoints[i] : localWaypoints[i] + transform.position;

                // draw a gizmo
                Gizmos.DrawLine(globalWaypointPos - Vector3.up * size, globalWaypointPos + Vector3.up * size);
                Gizmos.DrawLine(globalWaypointPos - Vector3.left * size, globalWaypointPos + Vector3.left * size);
            }
        }
    }
}
