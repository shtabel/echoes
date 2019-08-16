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
    LevelManager lvlManager;
    BlinkManager bm;  

    void Start()
    {
        lvlManager = FindObjectOfType<LevelManager>();
        bm = FindObjectOfType<BlinkManager>();
        
        AssignRBs();
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
                rb.velocity = rb.velocity / divInertia;

            }
        }
         
    }   

    public void BlowUpRocket()
    {
        // сначала отображаем взрыв
        bm.CreateBlink(bm.rocketBlown, transform.position);
        
        // потом уничтожаем саму ракету
        Destroy(gameObject);
    }

    void BlowUpMine(GameObject m)
    {       
        // сначала отображаем взрыв
        bm.CreateBlink(bm.mineBlown, m.transform.position);

        // потом уничтожаем саму мину
        Destroy(m);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "obstacle")
        {
            BlowUpRocket();
        }
        else if (other.tag == "mine")
        {
            BlowUpRocket();
            BlowUpMine(other.gameObject);

        }
        else if (other.tag == "rocket")
        {
            BlowUpRocket();

        }

        lvlManager.ResetArrays();
    }
    
}
