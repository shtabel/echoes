using System.Collections;
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

    new void Start()
    {
        base.Start();

        gameObject.GetComponent<MeshRenderer>().enabled = false;
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
            FaceTarget(targetPosition);

            Vector3 force = transform.right * thrust;
            rb.AddForce(force);

            float curDistancToPoint = Vector3.Distance(targetPosition, transform.position); // current distance to point

            if (curDistancToPoint < diactivateDistance)
            {
                startChasing = false;
                rb.velocity = rb.velocity / divInertia;

            }
        }
         
    }      

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "obstacle")
        {
            //BlowUpRocket();
        }
        else if (other.tag == "mine")
        {
            BlowUpEnemy();
            other.gameObject.GetComponent<EnemyController>().BlowUpEnemy();
        }
        else if (other.tag == "rocket")
        {
            BlowUpEnemy();
        }
        else if (other.tag == "generator")
        {
            other.GetComponent<GeneratorController>().DestroyGenerator();
            BlowUpEnemy();
        }

        //lvlManager.ResetArrays();
    }
    
}
