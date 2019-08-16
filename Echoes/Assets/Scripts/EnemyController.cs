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

    GameObject blinkType;

    // Start is called before the first frame update
    void Start()
    {
        AssignRBs();

        bm = FindObjectOfType<BlinkManager>();
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
