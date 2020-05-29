using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRadarController : MonoBehaviour
{
    // PUBLIC INIT
    [Range(-360f, 360f)]
    public float rotationDegree;    // speed of rotation
    public float rayLength;         // length of the ray
    public float rayWidth;          // width of the ray
    public bool showObstacles;      // to show green blinks

    public LayerMask playerMask;
    public LayerMask obstacleMask;
    public LayerMask rocketMask;
    public LayerMask sunkenMask;

    float showBlinksDst = 20;       // дистанция к игроку, на которой отображаем блинки препятствий

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

    BossBatleManager bossManager;

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

        bossManager = FindObjectOfType<BossBatleManager>();
    }

    // Update is called once per frame
    void Update()
    {
        // rotate ray
        transform.Rotate(0.0f, 0.0f, -rotationDegree * Time.deltaTime);

        // crate vector that we will allign our ray to
        Vector3 upVec = transform.TransformDirection(Vector3.up);
        Raycast(upVec);

        // get coordinates of the obstacle hit
        endCoord = RaycastObstacle(upVec);
        // draw the ray
        DrawRay(endCoord, transform.position + upVec * rayLength);
    }

    void Raycast(Vector3 vector)
    {
        RaycastPlayer(vector);
        RaycastRocket(vector);
        RaycastSunken(vector);
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

    void RaycastPlayer(Vector3 upVec)
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(transform.position, upVec, out hitInfo, rayLength, playerMask))
        {
            float dstToTarget = Vector3.Distance(transform.position, hitInfo.point);
            //Debug.Log("distance to player: " + dstToTarget);

            if (!Physics.Raycast(transform.position, upVec, dstToTarget, obstacleMask) && (dstToTarget <= rayLength))
            {
                thePlayer.DestroyPlayer();
            }
        }
    }

    void DestroySelf()
    {
        CreateExplosion(rayLength);

        transform.parent.gameObject.SetActive(false);

        bm.CreateBlink(bm.squareOrange, transform.position);
    }

    void RaycastRocket(Vector3 upVec)
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(transform.position, upVec, out hitInfo, rayLength, rocketMask))
        {
            float dstToTarget = Vector3.Distance(transform.position, hitInfo.point);

            if (!Physics.Raycast(transform.position, upVec, dstToTarget, obstacleMask) && (dstToTarget <= rayLength))
            {
                bossManager.LaunchNewRocket();

                hitInfo.collider.gameObject.GetComponent<RocketController>().BlowUpEnemy();
            }
        }
    }

    void RaycastSunken(Vector3 upVec)
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(transform.position, upVec, out hitInfo, rayLength, sunkenMask))
        {
            float dstToTarget = Vector3.Distance(transform.position, hitInfo.point);

            if (!Physics.Raycast(transform.position, upVec, dstToTarget, obstacleMask) && (dstToTarget <= rayLength))
            {
                bossManager.LaunchNewRocket();

                DestroySelf();
            }
        }
    }

    void CreateExplosion(float explosionRadius)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider nearbyObject in colliders)
        {
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);

                if (rb.gameObject.GetComponent<EnemyController>() != null)
                {
                    bm.CreateBlinkFollow(rb.gameObject.GetComponent<EnemyController>().blinkType[1], rb.transform.position, rb.gameObject);
                }

                if (Vector3.Distance(thePlayer.transform.position, transform.position) < 20)
                {
                    camShake.MediumShake();
                }
            }
        }
    }

    Vector3 RaycastObstacle(Vector3 upVec)
    {
        // draw ray in scene window
        //Debug.DrawRay(transform.position, upVec * rayLength, Color.red);

        RaycastHit hitInfo;
        float distanceBetweenBlinks = 0.5f;
        if (Physics.Raycast(transform.position, upVec, out hitInfo, rayLength, obstacleMask))
        {
            float dstToLastBlink = Vector3.Distance(lastBlinkPosition, hitInfo.point);
            if (dstToLastBlink >= distanceBetweenBlinks)
            {
                if (showObstacles && (Vector3.Distance(transform.position, thePlayer.transform.position) < showBlinksDst))
                {
                    // создаем блинк 
                    bm.CreateBlink(bm.blink, hitInfo.point);

                    lastBlinkPosition = hitInfo.point;
                }

            }

        }

        // if we hited obstacle - return hit's coordinates
        if (hitInfo.point != null)
        {
            return hitInfo.point;
        }

        return Vector3.zero;
    }
}
