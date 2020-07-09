using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRotationController : MonoBehaviour
{
    [SerializeField]
    float rotationSpeed;

    // Update is called once per frame
    void Update()
    {
        if (rotationSpeed != 0)
            transform.Rotate(0.0f, 0.0f, rotationSpeed * Time.deltaTime);
    }

    public void SetRotationSpeed(float speed)
    {
        rotationSpeed = speed;
    }
}
