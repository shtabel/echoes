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

    GameObject blinkType;

    // Start is called before the first frame update
    public void Start()
    {
        AssignRBs();
        rb = GetComponent<Rigidbody>();
        bm = FindObjectOfType<BlinkManager>();
    }

    public void CreateBlink(string tag)
    {
        if (nextTimeBlink < Time.time)
        {
            switch (tag)
            {
                case "mine":
                    bm.CreateBlink(bm.mine, transform.position);
                    break;
                case "rocket":
                    bm.CreateBlinkFollow(bm.rocket, transform.position, gameObject);
                    break;
                case "persuer":
                    bm.CreateBlink(bm.circleRed, transform.position);
                    break;
                case "runaway":
                    bm.CreateBlink(bm.circlePink, transform.position);
                    break;
            }

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
            bm.CreateBlink(bm.detectionBlink, positionToChase);
            nextTimeDetectBlink = Time.time + detectBlinkDelay;
        }
    }

    public void BlowUpMine()
    {
        // извлекаем rigidbody из списка
        EnemyRBs.Remove(rb);
        
        // отображаем взрыв
        bm.CreateBlink(bm.mineBlown, transform.position);

        // потом уничтожаем саму ракету
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        EnemyRBs.Remove(rb);
    }
}
