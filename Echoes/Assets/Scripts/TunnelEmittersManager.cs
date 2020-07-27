using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TunnelEmittersManager : MonoBehaviour
{
    [SerializeField]
    WaypointMovement[] movingEmitters;

    [SerializeField]
    GameObject emitterBase;

    bool activated;
    bool deactivated;
    
    float currentSpeed;
    [SerializeField]
    float minSpeed;
    [SerializeField]
    float maxSpeed;
    [SerializeField]
    float accelerationTime;
    float time;
    

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.O))
        //{
        //    ActivateEmitters();
        //}
        //if (Input.GetKeyDown(KeyCode.P))
        //{
        //    DeactivateEmitters();
        //}

        if (activated)
        {
            currentSpeed = Mathf.SmoothStep(minSpeed, maxSpeed, time / accelerationTime);
            for (int i = 0; i < movingEmitters.Length; i++)
            {
                movingEmitters[i].speed = currentSpeed;
            }

            time += Time.deltaTime;
        }
    }

    public void ActivateEmitters()
    {
        if (!activated)
        {
            for (int i = 0; i < movingEmitters.Length; i++)
            {
                movingEmitters[i].gameObject.SetActive(true);
            }

            activated = true;
        }
        
    }

    public void DeactivateEmitters()
    {
        if (!deactivated)
        {
            for (int i = 0; i < movingEmitters.Length; i++)
            {
                Instantiate(emitterBase, movingEmitters[i].transform.position, Quaternion.identity);
                movingEmitters[i].gameObject.SetActive(false);
            }

            deactivated = true;
        }
        
    }
}

