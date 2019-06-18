using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    // PUBLIC INIT
    public int additionBlinks;      // блинки которые следуют за первым
    public float blinksSpacing;     // разница между лучами в градусах

    public float rotationDegree;    // величина в градусах на которую вращается радар
    public float rayLength;         // длина луча
    public float[] blinkDelay = new float[2];        // задержка перед появлением следующего блинка (препятствия, мина)

    public GameObject blink;        // элемент блинк
    public GameObject mine;         // мина

    public LayerMask obstacleMask;  // маска преград
    public LayerMask mineMask;      // маска мин

    // private init
    float[] nextTimeBlink = new float[2];            // время след блинка (препятствия, мина)
      

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
        //Debug.DrawRay(transform.position, upVec * rayLength, Color.green);

        // if hit obstacle
        HandleObstacleBlink(upVec);
        Vector3 vec = upVec;// = Quaternion.AngleAxis(blinksSpacing, Vector3.forward) * upVec;        
        for (int i = 0; i < additionBlinks; i++)
        {
            vec = Quaternion.AngleAxis(blinksSpacing, Vector3.forward) * vec;
            HandleObstacleBlink(vec);
        }

        // if hit mine
        nextTimeBlink[1] = GetNextTimeToBlink(upVec, mineMask, mine, blinkDelay[1], nextTimeBlink[1], true);
    }

    void HandleObstacleBlink(Vector3 vector) // метод нужен для отображени блинков препятствий
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(transform.position, vector, out hitInfo, rayLength, obstacleMask))
        {
            Instantiate(blink, hitInfo.point, Quaternion.Euler(0, 0, 0));
        }
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
                if (!Physics.Raycast(transform.position, vector, dstToTarget, obstacleMask)) // если препятствие не перекрывает
                {
                    Instantiate(gameObj, hitInfo.point, Quaternion.Euler(0, 0, 0));
                    return timeToBlink = Time.time + bDelay;
                }
            }
            else
            {
                Instantiate(gameObj, hitInfo.point, Quaternion.Euler(0, 0, 0));
                return timeToBlink = Time.time + bDelay;
            }
            
        }

        return timeToBlink;
    }

}
