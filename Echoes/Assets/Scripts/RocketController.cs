using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketController : MonoBehaviour
{
    // PUBLIC INIT
    public float speedOfRocket;
    public Vector3 targetPos;
    public bool activate;

    public float activateDistance;

    public GameObject mineOr;
    public GameObject rocketOr;

    // PRIVATE INIT
    Transform player;
    LevelManager lvlManager;

    void Start()
    {
        PoolManager.instance.CreatePool(mineOr, 10);
        PoolManager.instance.CreatePool(rocketOr, 10);

        player = FindObjectOfType<PlayerController>().transform;
        lvlManager = FindObjectOfType<LevelManager>();
    }
    

    // Update is called once per frame
    void Update()
    {
        //float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        //Debug.Log("Distance to player: " + distanceToPlayer);
        //// если близко к игроку - ракета сама на него наводится
        //if (distanceToPlayer < activateDistance)
        //{
        //    activate = false;
        //    transform.position = Vector3.MoveTowards(transform.position, player.position, speedOfRocket * Time.deltaTime);
        //}
        //// когда игрок засек ракету, она активируется и движется к месту, на котором игрок ее засек
        //else 
        if (activate)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, speedOfRocket * Time.deltaTime);

        }


    }

    void BlowUpRocket()
    {
        // сначала отображаем взрыв
        PoolManager.instance.ReuseObject(rocketOr, transform.position, Quaternion.Euler(0, 0, 0));
        // потом уничтожаем саму ракету
        Destroy(gameObject);
    }

    void BlowUpMine(GameObject m)
    {
        // сначала отображаем взрыв
        PoolManager.instance.ReuseObject(mineOr, m.transform.position, Quaternion.Euler(0, 0, 0));
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
