using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    // PUBLIC INIT
    public int additionBlinks;      // блинки которые следуют за первым   
    public float blinksSpacing;     // разница между лучами в градусах
    
    public float[] distanceBetweenBlinks = new float[amountOfBlinks]; // расстояние между блинками

    public float rotationDegree;    // величина в градусах на которую вращается радар
    public float rayLength;         // длина луча
    public float[] blinkDelay = new float[amountOfBlinks];        // задержка перед появлением следующего блинка (препятствия, мина, ракета)
    
    public LayerMask obstacleMask;  // маска преград
    public LayerMask mineMask;      // маска мин
    public LayerMask rocketMask;      // маска ракет

    // private init
    static int amountOfBlinks = 3;         // количество типов блинков (препдствие, мина, ракета)
    float[] nextTimeBlink = new float[amountOfBlinks];            // время след блинка (препятствия, мина)
    Vector3 lastBlinkPosition;      // хранит позицию последнего блинка
    Vector3 lastMinePosition;       // хранит позицию последнего блинка мины
    Vector3 lastRocketPosition;       // хранит позицию последнего блинка ракеты

    BlinkManager bm;            // blink manager для создания блинков

    void Start()
    {
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
        //HandleObstacleBlink(upVec);
        Vector3 vec = upVec;// = Quaternion.AngleAxis(blinksSpacing, Vector3.forward) * upVec;        
        for (int i = 0; i <= additionBlinks; i++)
        {
            HandleObstacleBlink(vec);
            vec = Quaternion.AngleAxis(blinksSpacing, Vector3.forward) * vec;
            //HandleObstacleBlink(vec);
        }

        // if hit mine
        nextTimeBlink[1] = HandleMineBlink(upVec, nextTimeBlink[1], blinkDelay[1]);
        //nextTimeBlink[1] = GetNextTimeToBlink(upVec, mineMask, mine, blinkDelay[1], nextTimeBlink[1], true);

        // if hit rocket
        nextTimeBlink[2] = HandleRocketBlink(upVec, nextTimeBlink[2], blinkDelay[2]); 
    }

    void HandleObstacleBlink(Vector3 vector) // метод нужен для отображени блинков препятствий
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(transform.position, vector, out hitInfo, rayLength, obstacleMask))
        {
            float dstToLastBlink = Vector3.Distance(lastBlinkPosition, hitInfo.point);
            if (dstToLastBlink >= distanceBetweenBlinks[0])
            {
                //Instantiate(blink, hitInfo.point, Quaternion.Euler(0, 0, 0)); 
                //PoolManager.instance.ReuseObject(blink, hitInfo.point, Quaternion.Euler(0, 0, 0));
                bm.CreateBlink(bm.blink, hitInfo.point);

                lastBlinkPosition = hitInfo.point;
            }
           
        }
    }

    float HandleMineBlink(Vector3 vector, float timeToBlink, float bDelay)
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(transform.position, vector, out hitInfo, rayLength, mineMask))
        {
            float dstToTarget = Vector3.Distance(transform.position, hitInfo.point);
            float dstToLastMineBlink = Vector3.Distance(lastMinePosition, hitInfo.point);
            if (!Physics.Raycast(transform.position, vector, dstToTarget, obstacleMask) && ((dstToLastMineBlink >= distanceBetweenBlinks[1]) || (Time.time > timeToBlink))) // если препятствие не перекрывает
            {
                //Instantiate(mine, hitInfo.point, Quaternion.Euler(0, 0, 0));
                //PoolManager.instance.ReuseObject(mine, hitInfo.transform.position, Quaternion.Euler(0, 0, 0));
                bm.CreateBlink(bm.mine, hitInfo.transform.position);

                lastMinePosition = hitInfo.transform.position;
                return timeToBlink = Time.time + bDelay;
            }
        }
        return timeToBlink;
    }

    float HandleRocketBlink(Vector3 vector, float timeToBlink, float bDelay)
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(transform.position, vector, out hitInfo, rayLength, rocketMask))
        {
            float dstToTarget = Vector3.Distance(transform.position, hitInfo.point);
            float dstToLastRocketBlink = Vector3.Distance(lastRocketPosition, hitInfo.point);
            if (!Physics.Raycast(transform.position, vector, dstToTarget, obstacleMask) && ((dstToLastRocketBlink >= distanceBetweenBlinks[2]) || (Time.time > timeToBlink))) // если препятствие не перекрывает
            {
                //Instantiate(mine, hitInfo.point, Quaternion.Euler(0, 0, 0));
                //PoolManager.instance.ReuseObject(rocket, hitInfo.transform.position, Quaternion.Euler(0, 0, 0));
                bm.CreateBlink(bm.rocket, hitInfo.transform.position);

                lastRocketPosition = hitInfo.transform.position;
                
                // активируем ракету и передаем ей позицию игрока во время детектирования
                Vector3 targetPosition = transform.position;
                //hitInfo.collider.gameObject.GetComponent<RocketController>().activate = true;
                //hitInfo.collider.gameObject.GetComponent<RocketController>().targetPos = targetPosition;
                hitInfo.collider.gameObject.GetComponent<RocketController>().BeginChasing(targetPosition);

                // используем детекционный блинк, чтобы игрок видел куда направляется ракета
                //PoolManager.instance.ReuseObject(detectionBlink, targetPosition, Quaternion.Euler(0, 0, 0));
                bm.CreateBlink(bm.detectionBlink, targetPosition);

                return timeToBlink = Time.time + bDelay;
            }
        }
        return timeToBlink;
    }

    // метод отображает блинки в зависимости от маски с определенной задержкой и возвращает служующее время для блинка
    float GetNextTimeToBlink(Vector3 vector, LayerMask layerMsk, GameObject gameObj, float bDelay, float timeToBlink, bool invisibleZaStenoy)
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(transform.position, vector, out hitInfo, rayLength, layerMsk) && Time.time > timeToBlink)
        {
            if (invisibleZaStenoy)  // если объект невидим за препятствиями
            {
                float dstToTarget = Vector3.Distance(transform.position, hitInfo.point);
                float dstToLastBlink = Vector3.Distance(lastMinePosition, hitInfo.point);
                if (!Physics.Raycast(transform.position, vector, dstToTarget, obstacleMask) && (dstToLastBlink >= distanceBetweenBlinks[0])) // если препятствие не перекрывает
                {
                    Instantiate(gameObj, hitInfo.point, Quaternion.Euler(0, 0, 0));
                    //PoolManager.instance.ReuseObject(gameObj, hitInfo.point, Quaternion.Euler(0, 0, 0));
                    lastMinePosition = hitInfo.point;
                    return timeToBlink = Time.time + bDelay;
                }
            }
            else
            {
                Instantiate(gameObj, hitInfo.point, Quaternion.Euler(0, 0, 0));
                //PoolManager.instance.ReuseObject(gameObj, hitInfo.point, Quaternion.Euler(0, 0, 0));
                return timeToBlink = Time.time + bDelay;
            }
            
        }

        return timeToBlink;
    }

}
