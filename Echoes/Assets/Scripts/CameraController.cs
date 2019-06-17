using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // private init
    Vector3 targetPosition;
    PlayerController followTarget;
    
    void Start()
    {
        followTarget = FindObjectOfType<PlayerController>();
    }

    void Update()
    {
        targetPosition = new Vector3(followTarget.transform.position.x, followTarget.transform.position.y, followTarget.transform.position.z - 10);
        transform.position = targetPosition;
    }
}
