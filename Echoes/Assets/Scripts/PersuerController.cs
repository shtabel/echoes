using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersuerController : MonoBehaviour
{
    // PUBLIC INIT
    public float thrust;
    public float diactivateDistance;

    // PRIVATE INIT
    bool startChasing;
    Vector3 targetPosition;

    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {
        if (startChasing)
        {
            FaceTarget(targetPosition);

            Vector3 force = transform.right * thrust;
            rb.AddForce(force);

            float curDistancToPoint = Vector3.Distance(targetPosition, transform.position); // current distance to point

            if (curDistancToPoint < diactivateDistance)
            {
                startChasing = false;
            }
        }
    }

    void FaceTarget(Vector3 targetPos)
    {
        Vector3 difference = targetPos - transform.position;
        float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ);
    }

    public void ChaseToPosition(Vector3 positionToChase)
    {
        //Debug.Log("Target position: " + targetPosition.x + "; " + targetPosition.y);

        startChasing = true;
        targetPosition = positionToChase;
    }
}
