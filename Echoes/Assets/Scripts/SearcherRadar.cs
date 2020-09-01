using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearcherRadar : MonoBehaviour
{
    // PUBLIC INIT
    [Range(-360f, 360f)]
    public float rotationDegree;    // speed of rotation
    public float rayLength;         // length of the ray
    public float rayWidth;          // width of the ray
    public bool showObstacles;      // to show green blinks
    
    public LayerMask playerMask;
    public LayerMask obstacleMask;
    public LayerMask mineMask;
    public LayerMask rocketMask;
    public LayerMask sunkenMask;
    public LayerMask rayStopMask;

    float showBlinksDst = 20;       // дистанция у игроку, на которой отображаем блинки препятствий

    Vector3 endCoord;               // cordinates of the end of the ray when hitting obstacles

    [SerializeField]
    float explosionForce;           // сила отталкивания предметов при взрыве 

    // PRIVATE INIT
    //float blinkDelay = 0.4f;
    //float nextTimeBlink;

    Vector3 lastBlinkPosition;      // хранит позицию последнего блинка
    LineRenderer rayLineRenderer;
    BlinkManager bm;                // blink manager
    PlayerController thePlayer;
    CameraShake camShake;

    // Start is called before the first frame update
    void Start()
    {
        bm = FindObjectOfType<BlinkManager>();
        //nextTimeBlink = Time.time;
        thePlayer = FindObjectOfType<PlayerController>();
        camShake = FindObjectOfType<CameraShake>();

        // setting the line renderer
        rayLineRenderer = GetComponent<LineRenderer>();
        Vector3[] initRayPositions = new Vector3[2] { Vector3.zero, Vector3.zero };
        rayLineRenderer.SetPositions(initRayPositions);
        rayLineRenderer.SetWidth(rayWidth, rayWidth);

        showBlinksDst += rayLength;
    }

    // Update is called once per frame
    void Update()
    {
        // rotate ray
        transform.Rotate(0.0f, 0.0f, -rotationDegree * Time.deltaTime);

        if ((Vector3.Distance(transform.position, thePlayer.transform.position) < showBlinksDst))
        {
            // crate vector that we will allign our ray to
            Vector3 upVec = transform.TransformDirection(Vector3.up);
            Raycast(upVec);

            // get coordinates of the obstacle hit
            endCoord = RaycastObstacle(upVec);
            // draw the ray
            DrawRay(endCoord, transform.position + upVec * rayLength);
        }
        
    }

    void Raycast(Vector3 vector)
    {
        //RaycastPlayer(vector);
        RaycastMine(vector);
        RaycastRocket(vector);
        //RaycastSunken(vector);
    }
   
    void DrawRay(Vector3 endCoord1, Vector3 endCoord2)
    {
        // draw the ray
        rayLineRenderer.SetPosition(0, transform.position); // start coordinates
        if (endCoord != Vector3.zero)
        {
            rayLineRenderer.SetPosition(1, endCoord1);       // end obstacle hit coords
        }
        else
            rayLineRenderer.SetPosition(1, endCoord2); // end of the ray coords
    }

    void RaycastMine(Vector3 upVec)
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(transform.position, upVec, out hitInfo, rayLength, mineMask))
        {
            float dstToTarget = Vector3.Distance(transform.position, hitInfo.point);

            if (!Physics.Raycast(transform.position, upVec, dstToTarget, obstacleMask) && (dstToTarget <= rayLength)) 
               // && (Vector3.Distance(transform.position, thePlayer.transform.position) < showBlinksDst))
            {
                //Debug.Log("покажи мину");
                hitInfo.collider.gameObject.GetComponent<EnemyController>().CreateBlink();               
            }
        }
    }

    void RaycastPlayer(Vector3 upVec)
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(transform.position, upVec, out hitInfo, rayLength, playerMask))
        {
            float dstToTarget = Vector3.Distance(transform.position, hitInfo.point);
            //Debug.Log("distance to player: " + dstToTarget);

            if (!Physics.Raycast(transform.position, upVec, dstToTarget, obstacleMask) && (dstToTarget <= rayLength))
            {
                // поисковик засек игрока
                //Debug.Log("Player spottet");

                if (thePlayer.DestroyPlayer()) // if player not in the safe zone - destroy him and searcher
                    DestroySelf();

                
            }
        }
    }

    void DestroySelf()
    {
        CreateExplosion(rayLength);

        transform.parent.gameObject.SetActive(false);

        bm.CreateBlink(bm.blinkSquareOrange, transform.position);
    }

    void RaycastRocket(Vector3 upVec)
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(transform.position, upVec, out hitInfo, rayLength, rocketMask))
        {
            float dstToTarget = Vector3.Distance(transform.position, hitInfo.point);

            if (!Physics.Raycast(transform.position, upVec, dstToTarget, obstacleMask) && (dstToTarget <= rayLength))
                //&& (Vector3.Distance(transform.position, thePlayer.transform.position) < showBlinksDst))
            {
                // поисковик засек игрока
                //Debug.Log("Rocket spottet");

                DestroySelf();

                hitInfo.collider.gameObject.GetComponent<RocketController>().BlowUpEnemy(true);
            }
        }
    }
      

    void CreateExplosion(float explosionRadius)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider nearbyObject in colliders)
        {
            // собираем rb всех врагов в радиусе взрыва
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            if (rb != null && rb.gameObject.tag != "mine_boss")
            {
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);

                // если у rb это враг - создаем блинк
                if (rb.gameObject.GetComponent<EnemyController>() != null)                
                    bm.CreateBlinkFollow(rb.gameObject.GetComponent<EnemyController>().blinkType[1], rb.transform.position, rb.gameObject);                

                // если далеко от игрока - не трасем камеру
                if (Vector3.Distance(thePlayer.transform.position, transform.position) < showBlinksDst)                
                    camShake.MediumShake();
                
            }
        }
    }
        
    Vector3 RaycastObstacle(Vector3 upVec)
    {
        // draw ray in scene window
        //Debug.DrawRay(transform.position, upVec * rayLength, Color.red);

        RaycastHit hitInfo;
        RaycastHit hitInfo2;

        float distanceBetweenBlinks = 0.5f;
        if (Physics.Raycast(transform.position, upVec, out hitInfo, rayLength, obstacleMask))
        {
            float dstToLastBlink = Vector3.Distance(lastBlinkPosition, hitInfo.point);
            float dstToTarget = Vector3.Distance(transform.position, hitInfo.point);

            if (!Physics.Raycast(transform.position, upVec, out hitInfo2, dstToTarget, sunkenMask))
            {
                if (dstToLastBlink >= distanceBetweenBlinks)
                {
                    if (showObstacles)//&& (Vector3.Distance(transform.position, thePlayer.transform.position) < showBlinksDst)) 
                    {
                        // создаем блинк 
                        bm.CreateBlink(bm.blinkGreen, hitInfo.point);
                        lastBlinkPosition = hitInfo.point;
                        return hitInfo.point;
                    }
                }
            }
            else
            {
                // если обломок перекрывает стену - останавливаем им луч
                if (hitInfo2.collider.gameObject.GetComponent<EnemyController>())
                {
                    hitInfo2.collider.gameObject.GetComponent<EnemyController>().CreateBlink();
                }
                
                return hitInfo2.point;
            }         
        }

        // check sunken hit
        if (Physics.Raycast(transform.position, upVec, out hitInfo2, rayLength, sunkenMask))
        {
            float dstToTarget = Vector3.Distance(transform.position, hitInfo2.point);

            if (!Physics.Raycast(transform.position, upVec, dstToTarget, obstacleMask))
            {
                hitInfo2.collider.gameObject.GetComponent<EnemyController>().CreateBlink();

                return hitInfo2.point;
            }
        }

        // check player hit
        if (Physics.Raycast(transform.position, upVec, out hitInfo2, rayLength, playerMask))
        {
            float dstToTarget = Vector3.Distance(transform.position, hitInfo2.point);

            if (!Physics.Raycast(transform.position, upVec, dstToTarget, sunkenMask)
                && !Physics.Raycast(transform.position, upVec, dstToTarget, obstacleMask)) // Если на пути нет обломков
            {
                //Debug.Log(gameObject.name + " killed the player!");
                thePlayer.DestroyPlayer();

                //return hitInfo2.point;
            }
        }

        //if (Physics.Raycast(transform.position, upVec, out hitInfo2, rayLength, rayStopMask))
        //{
        //    float dstToTarget = Vector3.Distance(transform.position, hitInfo2.point);

        //    if (!Physics.Raycast(transform.position, upVec, dstToTarget, sunkenMask)
        //        && !Physics.Raycast(transform.position, upVec, dstToTarget, obstacleMask)) // Если на пути нет обломков
        //    {
        //        return hitInfo2.point;
        //    }
        //}

        // if we hited obstacle - return hit's coordinates
        if (hitInfo.point != null)  
        {            
            return hitInfo.point;
        }

        return transform.position;
    }
}
