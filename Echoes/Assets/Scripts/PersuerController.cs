using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersuerController : EnemyController
{
    // PUBLIC INIT
    public float thrust;
    public float diactivateDistance;

    public float repelRange;

    // PRIVATE INIT
    LevelManager lvlManager;
    BlinkManager bm;
    

    // Start is called before the first frame update
    void Start()
    {
        AssignRBs();

        lvlManager = FindObjectOfType<LevelManager>();
        bm = FindObjectOfType<BlinkManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (startChasing)
        {
            FaceTarget(targetPosition);

            Vector3 force = transform.right * thrust;
            //rb.AddForce(force);

            // отталкивание преследователей друг от друга, чтоб не сталкивались
            Vector3 repelForce = Vector3.zero;
            foreach (Rigidbody enemy in EnemyRBs)
            {
                if (enemy == rb)
                    continue;

                if (Vector3.Distance(enemy.position, rb.position) <= repelRange)
                {
                    repelForce += (rb.position - enemy.position).normalized;
                }
            }
            rb.AddForce(force + repelForce);

            float curDistancToPoint = Vector3.Distance(targetPosition, transform.position); // current distance to point

            if (curDistancToPoint < diactivateDistance)
            {
                startChasing = false;
            }
        }
    }

    public void BlowUpPersuer()
    {
        // сначала отображаем взрыв
        bm.CreateBlink(bm.circleBlown, transform.position);

        // потом уничтожаем саму ракету
        Destroy(transform.parent.gameObject);
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
        //if (other.tag == "obstacle")
        //{
        //    BlowUpPersuer();
        //}
        //else 
        if (other.tag == "mine")
        {
            BlowUpPersuer();
            BlowUpMine(other.gameObject);

        }
        else if (other.tag == "rocket")
        {
            BlowUpPersuer();
            other.GetComponent<RocketController>().BlowUpRocket();
        }
        if (other.tag == "persuer")
        {
            BlowUpPersuer();
        }

        lvlManager.ResetArrays();
    }
}
