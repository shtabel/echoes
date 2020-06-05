using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmitterToSpawner : MonoBehaviour
{
    [SerializeField]
    RocketSpawner rs;

    public void SpawnNewRocket()
    {
        rs.SpawnRocket();
    }
}
