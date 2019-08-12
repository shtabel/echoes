using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketController : MonoBehaviour
{
    // PUBLIC INIT
    public float thrust;        // тяга
    public float divInertia;    // значение в которое уменьшаем скорости при перестройке направления          

    public Vector3 targetPos;   // позиция цели (ирока)
    public bool isActivate;     // активна ли ракета

    public float activateDistance;  // расстояние на котором ракета сама наводится на цель

    public float diactivateDistance;  // расстояние до точки, где был замечен игрок, на котором на ракету перестает действовать сила

    // PRIVATE INIT
    Transform thePlayer;
    Rigidbody rb;
    LevelManager lvlManager;
    BlinkManager bm;

    void Start()
    {
        thePlayer = FindObjectOfType<PlayerController>().transform;
        lvlManager = FindObjectOfType<LevelManager>();
        bm = FindObjectOfType<BlinkManager>();

        rb = GetComponent<Rigidbody>();
    }
    
    public void BeginChasing(Vector3 targetPosition)
    {
        isActivate = true;
        targetPos = targetPosition;
        
        // это чтобы ракета при перестройке не тупила, а сразу меняла направление и нормально двигалась
        rb.velocity = rb.velocity / divInertia;
        //rb.angularVelocity = rb.angularVelocity / divInertia;

        FacePlayer(targetPos);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Vel: " + rb.velocity + "; ang vel: " + rb.angularVelocity);

        //// если близко к игроку - ракета сама на него наводится
        //if (distanceToPlayer < activateDistance)
        //{
        //    //isActivate = false;
        //    //transform.position = Vector3.MoveTowards(transform.position, thePlayer.position, speedOfRocket * Time.deltaTime);

        //    FacePlayer(thePlayer.position);
        //    Vector3 force = transform.right * thrust;
        //    rb.AddForce(force, ForceMode.Force);
        //}
        // когда игрок засек ракету, она активируется и движется к месту, на котором игрок ее засек
        //else
        if (isActivate)
        {
            //FacePlayer(thePlayer.position); // чтобы ракета постоянно наводилась в сторону игрока

            Vector3 force = transform.right * thrust;
            rb.AddForce(force);

            float curDistancToPoint = Vector3.Distance(targetPos, transform.position); // current distance to point

            if (curDistancToPoint < diactivateDistance)
            {
                isActivate = false;
            }
        }
         
    }

    void FacePlayer(Vector3 playerPos)
    {
        Vector3 difference = playerPos - transform.position;
        float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ);
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
