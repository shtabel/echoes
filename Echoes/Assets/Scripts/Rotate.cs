using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    // PUBLIC INIT
    //public int additionBlinks;      // блинки которые следуют за первым   
    //public float blinksSpacing;     // разница между лучами в градусах
    
    public float distanceBetweenBlinks; // расстояние между блинками

    [Range(0f, 360f)]
    public float rotationDegree;    // величина в градусах на которую вращается радар
    public float rayLength;         // длина луча
    
    public LayerMask obstacleMask;  // маска преград
    public LayerMask mineMask;      // маска мин
    public LayerMask rocketMask;      // маска ракет
    public LayerMask persuerMask;   // маска преследователя
    public LayerMask runawayMask;   // маска беглеца

    // private init
    enum TypeOfBlink {mine, rocket, persuer, runaway};
       

    Vector3 lastBlinkPosition;      // хранит позицию последнего блинка
    Vector3 lastMinePosition;       // хранит позицию последнего блинка мины
    Vector3 lastRocketPosition;       // хранит позицию последнего блинка ракеты
    Vector3 lastPersuerPosition;
    Vector3 lastRunawayPosition;

    MenuManager mm;
    BlinkManager bm;            // blink manager для создания блинков

    void Start()
    {
        mm = FindObjectOfType<MenuManager>();
        bm = FindObjectOfType<BlinkManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))    // change rotation direction
        {
            rotationDegree = rotationDegree * -1;
        }

        // rotate ray
        transform.Rotate(0.0f, 0.0f, -rotationDegree * Time.deltaTime);

        Raycast();  // cast ray       
    }

    public void Raycast()
    {
        Vector3 upVec = transform.TransformDirection(Vector3.up);

        // draw ray in scene window
        Debug.DrawRay(transform.position, upVec * rayLength, Color.green);

        // if hit obstacle
        HandleObstacleBlink(upVec);
        // это можно использовать если использую additional blinks и blink spacing между ними
        //Vector3 vec = upVec;// = Quaternion.AngleAxis(blinksSpacing, Vector3.forward) * upVec;        
        //for (int i = 0; i <= additionBlinks; i++)
        //{
        //HandleObstacleBlink(vec);
        //vec = Quaternion.AngleAxis(blinksSpacing, Vector3.forward) * vec;
        //HandleObstacleBlink(vec);
        //}

        // if hit mine
        //HandleMineBlink(upVec);
        HandleBlinks(upVec, mineMask, TypeOfBlink.mine, false);

        // if hit rocket
        //HandleRocketBlink(upVec);
        HandleBlinks(upVec, rocketMask, TypeOfBlink.rocket, true);

        // if hit persuer
        //HandlePersuerBlink(upVec);
        HandleBlinks(upVec, persuerMask, TypeOfBlink.persuer, true);

        // if hit runaway
        HandleBlinks(upVec, runawayMask, TypeOfBlink.runaway, true);
        //HandleRunawayBlink(upVec);
    }

    void HandleObstacleBlink(Vector3 vector) // метод нужен для отображени блинков препятствий
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(transform.position, vector, out hitInfo, rayLength, obstacleMask))
        {
            float dstToLastBlink = Vector3.Distance(lastBlinkPosition, hitInfo.point);
            if (dstToLastBlink >= distanceBetweenBlinks)
            {
                // создаем блинк 
                bm.CreateBlink(bm.blink, hitInfo.point);

                lastBlinkPosition = hitInfo.point;
            }
           
        }
    }

    void HandleBlinks(Vector3 vector, LayerMask layerMask, TypeOfBlink blinkType, bool detection)
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(transform.position, vector, out hitInfo, rayLength, layerMask))
        {
            float dstToTarget = Vector3.Distance(transform.position, hitInfo.point);
            if (!Physics.Raycast(transform.position, vector, dstToTarget, obstacleMask))
            {
                string tag = "";
                
                switch (blinkType)
                {
                    case TypeOfBlink.mine:
                        tag = "mine";
                        break;
                    case TypeOfBlink.rocket:
                        tag = "rocket";
                        break;
                    case TypeOfBlink.persuer:
                        tag = "persuer";
                        break;
                    case TypeOfBlink.runaway:
                        tag = "runaway";
                        break;
                }
                // создаем блинк
                hitInfo.collider.gameObject.GetComponent<EnemyController>().CreateBlink(tag);

                // активируем ракету и передаем ей позицию игрока во время детектирования
                if (detection)
                {
                    Vector3 targetPosition = transform.position;
                    hitInfo.collider.gameObject.GetComponent<EnemyController>().ChaseToPosition(targetPosition);
                }
            }
        }
    }

    void HandleMineBlink(Vector3 vector)
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(transform.position, vector, out hitInfo, rayLength, mineMask))
        {
            float dstToTarget = Vector3.Distance(transform.position, hitInfo.point);
            //float dstToLastMineBlink = Vector3.Distance(lastMinePosition, hitInfo.point);
            if (!Physics.Raycast(transform.position, vector, dstToTarget, obstacleMask))// && ((dstToLastMineBlink >= distanceBetweenBlinks[1]) || (Time.time > timeToBlink))) // если препятствие не перекрывает
            {
                // создаем блинк мины
                //bm.CreateBlink(bm.mine, hitInfo.transform.position);
                hitInfo.collider.gameObject.GetComponent<EnemyController>().CreateBlink("mine");

                //  отображаем информацию про мины
                if (!mm.infoDisplayed && mm.curLevel == "lvl2")
                {
                    mm.DisplayInfoCor();
                }

                //lastMinePosition = hitInfo.transform.position;
                //return timeToBlink = Time.time + bDelay;
            }
        }
        //return timeToBlink;
    }

    void HandleRocketBlink(Vector3 vector)
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(transform.position, vector, out hitInfo, rayLength, rocketMask))
        {
            float dstToTarget = Vector3.Distance(transform.position, hitInfo.point);
            float dstToLastRocketBlink = Vector3.Distance(lastRocketPosition, hitInfo.point);
            if (!Physics.Raycast(transform.position, vector, dstToTarget, obstacleMask))// && ((dstToLastRocketBlink >= distanceBetweenBlinks[2]) || (Time.time > timeToBlink))) // если препятствие не перекрывает
            {
                // создаем блинк ракеты
                // bm.CreateBlink(bm.rocket, hitInfo.transform.position);
                hitInfo.collider.gameObject.GetComponent<EnemyController>().CreateBlink("rocket");


                //  отображаем информацию про ракеты
                if (!mm.infoDisplayed && mm.curLevel == "lvl3")
                {
                    mm.DisplayInfoCor();
                }

                lastRocketPosition = hitInfo.transform.position;
                
                // активируем ракету и передаем ей позицию игрока во время детектирования
                Vector3 targetPosition = transform.position;
                hitInfo.collider.gameObject.GetComponent<EnemyController>().ChaseToPosition(targetPosition);

                // используем детекционный блинк, чтобы игрок видел куда направляется ракета
                bm.CreateBlink(bm.detectionBlink, targetPosition);
            }
        }
    }

    void HandlePersuerBlink(Vector3 vector)
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(transform.position, vector, out hitInfo, rayLength, persuerMask))
        {
            float dstToTarget = Vector3.Distance(transform.position, hitInfo.point);
            float dstToLastPersuerBlink = Vector3.Distance(lastPersuerPosition, hitInfo.point);
            if (!Physics.Raycast(transform.position, vector, dstToTarget, obstacleMask))// && ((dstToLastPersuerBlink >= distanceBetweenBlinks[3]) || (Time.time > timeToBlink))) // если препятствие не перекрывает
            {
                // создаем блинк ракеты
                //bm.CreateBlink(bm.circleRed, hitInfo.transform.position);
                hitInfo.collider.gameObject.GetComponent<EnemyController>().CreateBlink("persuer");

                lastPersuerPosition = hitInfo.transform.position;

                // активируем ракету и передаем ей позицию игрока во время детектирования
                Vector3 targetPosition = transform.position;
                hitInfo.collider.gameObject.GetComponent<EnemyController>().ChaseToPosition(targetPosition);

                // используем детекционный блинк, чтобы игрок видел куда направляется ракета
                bm.CreateBlink(bm.detectionBlink, targetPosition);
            }
        }
    }
      
    void HandleRunawayBlink(Vector3 vector)
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(transform.position, vector, out hitInfo, rayLength, runawayMask))
        {
            float dstToTarget = Vector3.Distance(transform.position, hitInfo.point);
            float dstToLastRunawayBlink = Vector3.Distance(lastRunawayPosition, hitInfo.point);
            if (!Physics.Raycast(transform.position, vector, dstToTarget, obstacleMask))// && ((dstToLastRunawayBlink >= distanceBetweenBlinks[4]) || (Time.time > timeToBlink))) // если препятствие не перекрывает
            {
                // создаем блинк ракеты
                //bm.CreateBlink(bm.circlePink, hitInfo.transform.position);
                hitInfo.collider.gameObject.GetComponent<EnemyController>().CreateBlink("runaway");


                lastRunawayPosition = hitInfo.transform.position;

                // активируем ракету и передаем ей позицию игрока во время детектирования
                Vector3 targetPosition = transform.position;

                //hitInfo.collider.gameObject.GetComponent<EnemyController>().ChaseToPosition(targetPosition);

                // используем детекционный блинк, чтобы игрок видел куда направляется ракета
                //bm.CreateBlink(bm.detectionBlink, targetPosition);
            }
        }
    }
}
