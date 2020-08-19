using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmitterScript : MonoBehaviour
{    
    [SerializeField]
    GameObject redBlink;    // child of the emitter to indicate endpoint of the laser

    // parameters of the ray
    [SerializeField]
    float rayWidth;         // ray width
    [SerializeField]
    float rayLength;        // ray length

    // layers masks to detect certain objects
    [SerializeField]
    LayerMask obstacleMask;
    [SerializeField]
    LayerMask sunkenMask;
    [SerializeField]
    LayerMask playerMask;
    [SerializeField]
    LayerMask rocketMask;
    [SerializeField]
    LayerMask mineMask;
    [SerializeField]
    LayerMask generatorMask;

    // references to objects
    BlinkManager bm;
    LineRenderer rayLineRenderer;
    PlayerController thePlayer;

    Vector3 endCoord;   // end coordinate of the line renderer

    [SerializeField]
    bool showObstacles;
    Vector3 lastBlinkPosition;      // хранит позицию последнего блинка
    float showBlinksDst = 25;       // дистанция у игроку, на которой отображаем блинки препятствий

    [SerializeField]
    float sunkenDragHold;           // с какой силой удерживаем обломок

    Transform _selection;   // to deactivate generator

    // Start is called before the first frame update
    void Start()
    {
        bm = FindObjectOfType<BlinkManager>();
        thePlayer = FindObjectOfType<PlayerController>();

        // setting the line renderer
        rayLineRenderer = GetComponent<LineRenderer>();
        Vector3[] initRayPositions = new Vector3[2] { Vector3.zero, Vector3.zero };
        rayLineRenderer.SetPositions(initRayPositions);
        rayLineRenderer.SetWidth(rayWidth, rayWidth);   
    }

    // Update is called once per frame
    void Update()
    {
        // crate vector that we will allign our ray to
        Vector3 upVec = transform.TransformDirection(Vector3.up);

        //Debug.DrawRay(transform.position, upVec * rayLength, Color.red);

        if ((Vector3.Distance(transform.position, thePlayer.transform.position) < showBlinksDst))
        {
            endCoord = Raycast(upVec);

            rayLineRenderer.SetPosition(0, transform.position);
            rayLineRenderer.SetPosition(1, endCoord);
        }
        
    }

    Vector3 Raycast(Vector3 vector)
    {
        RaycastHit hitInfo1; // hit for obstacles
        RaycastHit hitInfo2; // hot for the player and sunkens

        // check obstacles
        float distanceBetweenBlinks = 0.7f;
        if (Physics.Raycast(transform.position, vector, out hitInfo2, rayLength, obstacleMask))
        {
            float dstToLastBlink = Vector3.Distance(lastBlinkPosition, hitInfo2.point);
            if (dstToLastBlink >= distanceBetweenBlinks)
            {
                if (showObstacles)//&& (Vector3.Distance(transform.position, thePlayer.transform.position) < showBlinksDst)) 
                {
                    // создаем блинк 
                    bm.CreateBlink(bm.blinkGreen, hitInfo2.point);

                    lastBlinkPosition = hitInfo2.point;
                }
            }
        }

        // check player hit
        if (Physics.Raycast(transform.position, vector, out hitInfo2, rayLength, playerMask))
        {
            float dstToTarget = Vector3.Distance(transform.position, hitInfo2.point);

            if (!Physics.Raycast(transform.position, vector, dstToTarget, sunkenMask) 
                && !Physics.Raycast(transform.position, vector, dstToTarget, obstacleMask)) // Если на пути нет обломков
            {
                //Debug.Log(gameObject.name + " killed the player!");
                if(thePlayer.DestroyPlayer());
                SetRedBlinkPosition(hitInfo2.point);
                return hitInfo2.point;

            }
        }


        // check mine hit
        RaycastHit[] hitMines;
        hitMines = Physics.RaycastAll(transform.position, vector, rayLength, mineMask);

        for (int i = 0; i < hitMines.Length; i++)
        {
            RaycastHit hit = hitMines[i];
            float dstToTarget = Vector3.Distance(transform.position, hit.point);

            if (!Physics.Raycast(transform.position, vector, dstToTarget, sunkenMask)
                && !Physics.Raycast(transform.position, vector, dstToTarget, obstacleMask)) // Если на пути нет обломков
            {
                hit.transform.gameObject.GetComponent<EnemyController>().CreateBlink();
                //return hitInfo2.point;
            }
        }

        // check rocket hit
        if (Physics.Raycast(transform.position, vector, out hitInfo2, rayLength, rocketMask))
        {
            float dstToTarget = Vector3.Distance(transform.position, hitInfo2.point);

            if (!Physics.Raycast(transform.position, vector, dstToTarget, sunkenMask)
                && !Physics.Raycast(transform.position, vector, dstToTarget, obstacleMask)) // Если на пути нет обломков
            {
                //Debug.Log(gameObject.name + " killed the player!");
                hitInfo2.collider.gameObject.GetComponent<RocketController>().BlowUpEnemy(true);

                if (GetComponent<EmitterToSpawner>())
                {
                    GetComponent<EmitterToSpawner>().SpawnNewRocket();
                }

                return hitInfo2.point;
            }
        }

        // check generator v2 hit
        if (_selection != null)
        {
            if (_selection.tag == "generator")
                _selection.GetComponent<Generatorv2Controller>().DeactivateGenerator();
            if (_selection.tag == "BBeaconEmitter")
                _selection.GetComponent<BBeaconEmitterController>().Deactivate();

            //_selection.GetComponent<Generatorv2Controller>().DeactivateGenerator();
            _selection = null;
        }
        if (Physics.Raycast(transform.position, vector, out hitInfo2, rayLength, generatorMask))
        {
            float dstToTarget = Vector3.Distance(transform.position, hitInfo2.point);

            if (!Physics.Raycast(transform.position, vector, dstToTarget, sunkenMask)
                && !Physics.Raycast(transform.position, vector, dstToTarget, obstacleMask)) // Если на пути нет обломков
            {
                //Debug.Log("Generator hit");
                SetRedBlinkPosition(hitInfo2.point);

                var selection = hitInfo2.transform;

                if (selection.tag == "generator")
                    selection.GetComponent<Generatorv2Controller>().ActivateGenerator();
                if (selection.tag == "BBeaconEmitter")
                    selection.GetComponent<BBeaconEmitterController>().Activate();
                if (selection.tag == "SBeaconMEmitter")
                    selection.GetComponent<BeaconController>().ActivateBeacon();
                if (selection.tag == "BBeaconMEmitter")
                    selection.GetComponent<Beaconv2Controller>().ActivateBeacon();
                

                _selection = selection;
                
                return hitInfo2.point;
            }
        }

        // check obstacle and sunken hit
        if (Physics.Raycast(transform.position, vector, out hitInfo1, rayLength, obstacleMask))
        {  
            // if hit sunken
            if (Physics.Raycast(transform.position, vector, out hitInfo2, rayLength, sunkenMask))
            {
                float dstToTarget = Vector3.Distance(transform.position, hitInfo2.point);
                if (!Physics.Raycast(transform.position, vector, dstToTarget, obstacleMask)) // Если на пути нет препятствий
                {
                    var selection = hitInfo2.transform;
                    //selection.GetComponent<Rigidbody>().drag = sunkenDragHold;
                    selection.GetComponent<EnemyController>().ChangeDrag(sunkenDragHold, false);


                    hitInfo2.transform.gameObject.GetComponent<EnemyController>().CreateBlink();
                    SetRedBlinkPosition(hitInfo2.point);
                    return hitInfo2.point;
                }
            }

            SetRedBlinkPosition(hitInfo1.point);
            return hitInfo1.point;
        }

        return (transform.position);
    }

    void SetRedBlinkPosition(Vector3 pos)
    {
        redBlink.transform.position = pos;
    }
}
