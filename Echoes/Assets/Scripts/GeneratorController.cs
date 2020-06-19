using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorController : MonoBehaviour // генератор, который питает закрытую дверь и его нужно уничтожить
{
    [SerializeField]
    float explosForce;  // explosion force
    [SerializeField]
    float explosRadius; // explosion radius

    [SerializeField]
    GameObject genMng;
    BlinkManager bm;    

    // Start is called before the first frame update
    void Start()
    {
        //genMng = FindObjectOfType<GeneratorManager>();
        bm = FindObjectOfType<BlinkManager>();        
    }
    
    public void DestroyGenerator()
    {
        CreateExplosion(explosForce, explosRadius);
        HandleManager();    // tell the particular manager that we've destroyed one generator
        bm.CreateBlink(bm.blinkCircleOrange, transform.position);
        Destroy(gameObject);
    }

    void HandleManager()
    {
        if (genMng.GetComponent<GeneratorManager>())
        {
            genMng.GetComponent<GeneratorManager>().MinusGenerator();
        } else if (genMng.GetComponent<BossBatleManager>())
        {
            genMng.GetComponent<BossBatleManager>().MinusGenerator();
        }
    }

    void CreateExplosion(float explosionForce, float radiusOfExplosion)
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
}
