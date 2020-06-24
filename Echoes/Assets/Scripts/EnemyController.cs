using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // PUBLIC INIT    
    public bool startChasing;
    public Vector3 targetPosition;
    
    public BlinkManager bm;
    public Rigidbody rb;

    public static List<Rigidbody> EnemyRBs;

    // PRIVATE INIT
    float blinkGap = 1;
    float nextTimeBlink;

    float detectBlinkDelay = 0.4f;
    float nextTimeDetectBlink;

    public GameObject[] blinkType = new GameObject[2];

    [SerializeField]
    int explosionForce;
    [SerializeField]
    int explosionRadius;

    LevelManager lvlManager;

    CameraShake camShake;

    float initDrag;

    // Start is called before the first frame update
    public void Start()
    {
        AssignRBs();
        rb = GetComponent<Rigidbody>();

        bm = FindObjectOfType<BlinkManager>();
        lvlManager = FindObjectOfType<LevelManager>();        
        camShake = FindObjectOfType<CameraShake>();

        blinkType = AssignIcon();
        
        bm.CreateBlinkFollow(blinkType[0], transform.position, gameObject);

        if (gameObject.tag == "sunken")
            initDrag = GetComponent<Rigidbody>().drag;
    }

    GameObject[] AssignIcon()
    {
        GameObject[] blink = new GameObject[2];

        switch (gameObject.tag)
        {
            case "mine":
                blink[0] = bm.blinkCrossRed;
                blink[1] = bm.blinkCrossOrange;
                break;
            case "rocket":
                blink[0] = bm.blinkTriangleRed;
                blink[1] = bm.blinkTriangleOrange; ;
                break;
            case "persuer":
                blink[0] = bm.blinkCircleRed;
                blink[1] = bm.blinkCircleOrange;
                break;
            case "runaway":
                blink[0] = bm.blinkCirclePink;
                blink[1] = bm.blinkCircleOrange;
                break;
            case "sunken":
                blink[0] = bm.blinkCircleGray;
                blink[1] = bm.blinkCircleGray;
                break;
            case "door":
                blink[0] = bm.blinkGray;
                blink[1] = bm.blinkGray;
                break;
        }

        return blink;
    }

    public void CreateBlink()
    {
        if (nextTimeBlink < Time.time)
        {
            if (gameObject.tag == "mine" && !GetComponent<WaypointMovement>())
            {
                bm.CreateBlink(blinkType[0], transform.position);
            }
            else
                bm.CreateBlinkFollow(blinkType[0], transform.position, gameObject);

            nextTimeBlink = Time.time + blinkGap;
        }        
    }

    public void SetKinematic(bool isKinematic)
    {
        rb.isKinematic = isKinematic;
    }

    public void AssignRBs()
    {
        rb = GetComponent<Rigidbody>();

        if (EnemyRBs == null)
        {
            EnemyRBs = new List<Rigidbody>();
        }
        EnemyRBs.Add(rb);
    }

    public void FaceTarget(Vector3 targetPos)
    {
        Vector3 difference = targetPos - transform.position;
        float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ);
    }

    public void ChaseToPosition(Vector3 positionToChase)
    {
        startChasing = true;
        targetPosition = positionToChase;

        if (nextTimeDetectBlink < Time.time)
        {
            bm.CreateBlink(bm.blinkRed, positionToChase);
            nextTimeDetectBlink = Time.time + detectBlinkDelay;
        }
    }

    public void BlowUpEnemy()
    {      
        if (Vector3.Distance(FindObjectOfType<PlayerController>().transform.position, transform.position) < 20)
        {
            // создаем взрыв
            CreateExplosion(explosionRadius);

            // отображаем взрыв
            bm.CreateBlink(blinkType[1], transform.position);

            camShake.MediumShake();
        }

        // потом уничтожаем сам объект
        Destroy(gameObject);

        lvlManager.ResetArrays();
    }

    private void OnDestroy()
    {
        EnemyRBs.Remove(rb);
    }

    void CreateExplosion(float radiusOfExplosion)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radiusOfExplosion);
        foreach (Collider nearbyObject in colliders)
        {
            Rigidbody rigidBody = nearbyObject.GetComponent<Rigidbody>();
            if (rigidBody != null && (rigidBody.tag == "sunken" || rigidBody.tag == "Player"))
            {
                // apply explosion force
                rigidBody.AddExplosionForce(explosionForce, transform.position, radiusOfExplosion);

                // show blinks    
                if (rigidBody.tag == "sunken")    
                    bm.CreateBlinkFollow(rigidBody.gameObject.GetComponent<EnemyController>().blinkType[1], rigidBody.transform.position, rigidBody.gameObject);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // если обломок сталкивается с другими врагами - взорви их
        if (gameObject.tag == "sunken" && (other.tag == "mine" || other.tag == "rocket" || other.tag == "persuer"))
        {
            other.gameObject.GetComponent<EnemyController>().BlowUpEnemy();
            lvlManager.ResetArrays();
        }

        // если обломок сталкивается с генератором
        if (gameObject.tag == "sunken" && other.tag == "generator")
        {
            other.GetComponent<GeneratorController>().DestroyGenerator();
            Debug.Log("обломок сталкивается с генератором ");
        }

    }

    void OnCollisionEnter(Collision collision)
    {
        // если обломок сталкивается с обломком - покажи его
        if (collision.gameObject.tag == "sunken" && gameObject.tag == "sunken")
        {
            //bm.CreateBlinkFollow(blinkType[0], collision.transform.position, collision.gameObject);
            bm.CreateBlinkFollow(blinkType[0], transform.position, gameObject);
        }

        // если обломок сталкивается с игроком - drag как сначала
        if (collision.gameObject.tag == "Player" && gameObject.tag == "sunken")
        {
            gameObject.GetComponent<Rigidbody>().drag = initDrag;
        }

        // если обломок сталкивается со стеной - показываем значек
        if (collision.gameObject.tag == "obstacle" && gameObject.tag == "sunken")
        {
            bm.CreateBlinkFollow(blinkType[0], transform.position, gameObject);
        }

        //if (gameObject.tag == "sunken")
        //{
        //    bm.CreateBlinkFollow(blinkType[0], transform.position, gameObject);
        //}
    }

}
