using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketSpawner : MonoBehaviour
{
    [SerializeField]
    GameObject rocket;

    void Start()
    {
        SpawnRocket();
    }

    public void SpawnRocket()
    {
        Instantiate(rocket, transform.position, Quaternion.identity);
    }
}
