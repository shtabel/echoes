﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketController : EnemyController
{
    // PUBLIC INIT
    public float thrust;        // тяга
    public float divInertia;    // значение в которое уменьшаем скорости при перестройке направления          
    
    public float activateDistance;  // расстояние на котором ракета сама наводится на цель

    public float diactivateDistance;  // расстояние до точки, где был замечен игрок, на котором на ракету перестает действовать сила

    // PRIVATE INIT   
    public bool detected1;             // засекли мину ракету первый раз
    bool detected;

    AudioManager am;

    new void Start()
    {
        base.Start();

        gameObject.GetComponent<MeshRenderer>().enabled = false;
        am = FindObjectOfType<AudioManager>();

        // если это ракета босса
        if (transform.position.y > -229)
            MakeSound();
    }

    public void BeginChasing(Vector3 targetPos)
    {
        startChasing = true;
        targetPosition = targetPos;
        
        // это чтобы ракета при перестройке не тупила, а сразу меняла направление и нормально двигалась
        rb.velocity = rb.velocity / divInertia;
        //rb.angularVelocity = rb.angularVelocity / divInertia;

        FaceTarget(targetPosition);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (startChasing)
        {            
            float curDistancToPoint = Vector3.Distance(targetPosition, transform.position); // current distance to point

            //if(!detected)
            //    Debug.Log("dist = " + curDistancToPoint);

            FaceTarget(targetPosition);

            Vector3 force = transform.right * thrust;
            if (curDistancToPoint < 6 && !detected1)
                force = transform.right * thrust / 2;

            //if (!detected)
            //    Debug.Log("force = " + Mathf.Pow(force.x*force.x + force.y*force.y, 0.5f));

            rb.AddForce(force);

            MakeSound();
            detected = true;       

            if (curDistancToPoint < diactivateDistance)
            {
                startChasing = false;
                detected = false;
                rb.velocity = rb.velocity / divInertia;
            }


        }
         
    }      

    public void MakeSound()
    {
        if (!detected1)
        {
            am.Play("rocket_spot1");
            detected1 = true;
            return;
        }
        else if (!detected)
        {
            am.Play("rocket_spot");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "obstacle")
        {
            //BlowUpRocket();
        }
        else if (other.tag == "mine" )//|| other.tag == "mine_boss")
        {
            BlowUpEnemy(true);
            other.gameObject.GetComponent<EnemyController>().BlowUpEnemy(false);
        }
        else if (other.tag == "rocket")
        {
            BlowUpEnemy(true);
        }
        else if (other.tag == "generator")
        {
            other.GetComponent<GeneratorController>().DestroyGenerator();
            BlowUpEnemy(false);
        }

        //lvlManager.ResetArrays();
    }
    
}
