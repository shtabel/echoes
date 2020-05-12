using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    SaveManager sm;

    void Start()
    {
        sm = FindObjectOfType<SaveManager>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerPrefs.SetFloat("lastCheckpointPosX", transform.position.x);
            PlayerPrefs.SetFloat("lastCheckpointPosY", transform.position.y);
            PlayerPrefs.SetFloat("lastCheckpointPosZ", transform.position.z);
        }
    }
}
