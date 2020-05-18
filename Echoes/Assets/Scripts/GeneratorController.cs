using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorController : MonoBehaviour
{
    [SerializeField]
    float explosForce;  // explosion force
    [SerializeField]
    float explosRadius; // explosion radius

    GeneratorManager genMng;
    BlinkManager bm;    

    // Start is called before the first frame update
    void Start()
    {
        genMng = FindObjectOfType<GeneratorManager>();
        bm = FindObjectOfType<BlinkManager>();
    }
    
    public void DestroyGenerator()
    {
        CreateExplosion(explosForce, explosRadius);
        genMng.MinusGenerator();
        bm.CreateBlink(bm.circleBlown, transform.position);
        Destroy(gameObject);
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
