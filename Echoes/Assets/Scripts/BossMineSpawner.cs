using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMineSpawner : MonoBehaviour
{
    [SerializeField]
    GameObject minePrefab;

    [SerializeField]
    float mineSpeed;

    [SerializeField]
    float spawnDelay;
    float nextTimeSpawn;


    [SerializeField]
    float rotationSpeed;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0.0f, 0.0f, rotationSpeed * Time.deltaTime);

        if (Time.time >= nextTimeSpawn)
        {
            // spawned mine
            GameObject mineSpawned = Instantiate(minePrefab, transform.position, transform.rotation);

            //mineSpawned.GetComponent<Rigidbody>().AddForce(gameObject.tr, ForceMode.Acceleration);
            mineSpawned.GetComponent<Rigidbody>().AddRelativeForce(Vector3.up * mineSpeed, ForceMode.Acceleration);
            //mineSpawned.transform.Translate(Vector3.forward * mineSpeed * Time.deltaTime);

            nextTimeSpawn = Time.time + spawnDelay;
        }

    }
}
