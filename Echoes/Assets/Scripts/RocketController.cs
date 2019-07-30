using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketController : MonoBehaviour
{
    // PUBLIC INIT
    public float speedOfRocket;
    public Vector3 targetPos;
    public bool activate;


    // PRIVATE INIT

    

    // Update is called once per frame
    void Update()
    {
        if (activate)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, speedOfRocket * Time.deltaTime);

        }


    }
}
