using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    // PUBLIC INIT
    //public int additionBlinks;      // блинки которые следуют за первым   
    //public float blinksSpacing;     // разница между лучами в градусах
    
    public float distanceBetweenBlinks; // расстояние между блинками
    public float dstBetweenBlueBlinks;

    [Range(0f, 360f)]
    public float rotationDegree;    // величина в градусах на которую вращается радар
    public float rayLength;         // длина луча
    public float rayWidth;          // width of the ray

    public LayerMask obstacleMask;  // маска преград
    public LayerMask mineMask;      // маска мин
    public LayerMask rocketMask;      // маска ракет
    public LayerMask persuerMask;   // маска преследователя
    public LayerMask runawayMask;   // маска беглеца
    public LayerMask sunkenMask;   // маска затопленного
    public LayerMask beaconMask;   // маска маячка
    public LayerMask doorMask;   // маска двери
    public LayerMask emitterMask;   // маска эмиттеров

    Vector3 endCoord;               // cordinates of the end of the ray when hitting obstacles
    LineRenderer rayLineRenderer;

    // private init
    //enum TypeOfBlink {mine, rocket, persuer, runaway, sunken};
       

    Vector3 lastBlinkPosition;      // хранит позицию последнего блинка
    Vector3 lastBlueBlinkPosition;

    MenuManager mm;
    BlinkManager bm;            // blink manager для создания блинков
    ChatManager cm;

    void Start()
    {
        mm = FindObjectOfType<MenuManager>();
        bm = FindObjectOfType<BlinkManager>();
        cm = FindObjectOfType<ChatManager>();

        // setting the line renderer
        rayLineRenderer = GetComponent<LineRenderer>();
        Vector3[] initRayPositions = new Vector3[2] { Vector3.zero, Vector3.zero };
        rayLineRenderer.SetPositions(initRayPositions);
        rayLineRenderer.SetWidth(rayWidth, rayWidth);
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.R))    // change rotation direction
        //{
        //    rotationDegree = rotationDegree * -1;
        //}

        // rotate ray
        transform.Rotate(0.0f, 0.0f, -rotationDegree * Time.deltaTime);

        // crate vector that we will allign our ray to
        Vector3 upVec = transform.TransformDirection(Vector3.up);
        endCoord = HandleObstacleBlink(upVec);
        DetectObjects(upVec);  // cast ray    

        // draw the ray
        DrawRay(endCoord, transform.position + upVec * rayLength);
    }

    void DrawRay(Vector3 endCoord1, Vector3 endCoord2)
    {
        // draw the ray
        rayLineRenderer.SetPosition(0, transform.position); // start coordinates
        
        //if (endCoord != Vector3.zero)
        //{
        //    rayLineRenderer.SetPosition(1, endCoord1);       // end obstacle hit coords
        //}
        //else
            rayLineRenderer.SetPosition(1, endCoord2); // end of the ray coords
    }   

    public void DetectObjects(Vector3 vec)
    {
        //Vector3 upVec = transform.TransformDirection(Vector3.up);

        // draw ray in scene window
        //Debug.DrawRay(transform.position, upVec * rayLength, Color.green);

        // if hit obstacle
        //HandleObstacleBlink(upVec);
        //// это можно использовать если использую additional blinks и blink spacing между ними
        //Vector3 vec = upVec;// = Quaternion.AngleAxis(blinksSpacing, Vector3.forward) * upVec;        
        //for (int i = 0; i <= additionBlinks; i++)
        //{
        //HandleObstacleBlink(vec);
        //vec = Quaternion.AngleAxis(blinksSpacing, Vector3.forward) * vec;
        //HandleObstacleBlink(vec);
        //}

        // if hit mine
        //HandleMineBlink(upVec);
        HandleBlinks(vec, mineMask, false);

        // if hit rocket
        //HandleRocketBlink(upVec);
        HandleBlinks(vec, rocketMask, true);

        // if hit persuer
        //HandlePersuerBlink(upVec);
        HandleBlinks(vec, persuerMask, true);

        // if hit runaway
        HandleBlinks(vec, runawayMask, true);
        //HandleRunawayBlink(upVec);

        // if hit sunken
        HandleBlinks(vec, sunkenMask, false);

        HandleBeacon(vec);
        HandleDoor(vec);
    }

    Vector3 HandleObstacleBlink(Vector3 vector) // метод нужен для отображени блинков препятствий
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(transform.position, vector, out hitInfo, rayLength, obstacleMask))
        {
            float dstToLastBlink = Vector3.Distance(lastBlinkPosition, hitInfo.point);
            float dstToTarget = Vector3.Distance(transform.position, hitInfo.point);

            if (dstToLastBlink >= distanceBetweenBlinks 
                && (!Physics.Raycast(transform.position, vector, dstToTarget, doorMask) 
                && !Physics.Raycast(transform.position, vector, dstToTarget, emitterMask)))
            {
                // создаем блинк 
                bm.CreateBlink(bm.blinkGreen, hitInfo.point);
                
                lastBlinkPosition = hitInfo.point;
            }

            // if we hited obstacle - return hit's coordinates
            if (hitInfo.point != null)
            {
                return hitInfo.point;
            }
        }

        return Vector3.zero;
    }

    void HandleDoor(Vector3 vector)
    {
        RaycastHit hitInfo;

        if (Physics.Raycast(transform.position, vector, out hitInfo, rayLength, doorMask))
        {
            float dstToLastBlueBlink = Vector3.Distance(lastBlueBlinkPosition, hitInfo.point);
            float dstToTarget = Vector3.Distance(transform.position, hitInfo.point);

            if (dstToLastBlueBlink >= dstBetweenBlueBlinks && !Physics.Raycast(transform.position, vector, dstToTarget, obstacleMask))
            {
                bm.CreateBlinkFollow(bm.blinkGray, hitInfo.collider.transform.position, hitInfo.collider.gameObject);
                lastBlueBlinkPosition = hitInfo.point;
            }
        }        
    }

    void ShowInfo(string tag)
    {
        //  отображаем информацию про мины если второй уровень
        if (!mm.infoDisplayed && mm.curLevel == "lvl2" && tag == "mine")
        {
            mm.DisplayInfoCor();
        }
        //  отображаем информацию про ракеты если 3 уровень
        else if (!mm.infoDisplayed && mm.curLevel == "lvl3" && tag == "rocket")
        {
            mm.DisplayInfoCor();
        }

    }

    void HandleBlinks(Vector3 vector, LayerMask layerMask, bool chasePlayer)
    {
        RaycastHit[] hitInfo;

        hitInfo = Physics.RaycastAll(transform.position, vector, rayLength, layerMask);

        for (int i = 0; i < hitInfo.Length; i++)
        {
            RaycastHit hit = hitInfo[i];

            float dstToTarget = Vector3.Distance(transform.position, hit.point);
            if (!Physics.Raycast(transform.position, vector, dstToTarget, obstacleMask))
            {
                string tag = "";

                //switch (blinkType)
                //{
                //    case TypeOfBlink.mine:
                //        tag = "mine";
                //        break;
                //    case TypeOfBlink.rocket:
                //        tag = "rocket";
                //        break;
                //    case TypeOfBlink.persuer:
                //        tag = "persuer";
                //        break;
                //    case TypeOfBlink.runaway:
                //        tag = "runaway";
                //        break;
                //    case TypeOfBlink.sunken:
                //        tag = "sunken";
                //        break;
                //}

                // создаем блинк
                hit.collider.gameObject.GetComponent<EnemyController>().CreateBlink();

                if (hit.collider.gameObject.tag == "mine")
                    cm.DisplayMessage("mine");

                ShowInfo(tag);

                // активируем ракету и передаем ей позицию игрока во время детектирования
                if (chasePlayer)
                {
                    Vector3 targetPosition = transform.position;
                    hit.collider.gameObject.GetComponent<EnemyController>().ChaseToPosition(targetPosition);
                }
            }
        }
    }

    void HandleBeacon(Vector3 vector)
    {
        RaycastHit[] hitInfo;

        hitInfo = Physics.RaycastAll(transform.position, vector, rayLength, beaconMask);

        for (int i = 0; i < hitInfo.Length; i++)
        {
            RaycastHit hit = hitInfo[i];

            float dstToTarget = Vector3.Distance(transform.position, hit.point);
            if (!Physics.Raycast(transform.position, vector, dstToTarget, obstacleMask))
            {
                
                // activate beacon
                hit.collider.gameObject.GetComponent<BeaconController>().ActivateBeacon();
                
            }
        }
    }

    
}
