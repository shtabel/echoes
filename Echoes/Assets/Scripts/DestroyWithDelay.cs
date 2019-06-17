using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyWithDelay : MonoBehaviour
{
    // PUBLIC INIT
    public float lifeTime;

    // Update is called once per frame
    void Update()
    {
        Destroy(gameObject, lifeTime);
    }
}
