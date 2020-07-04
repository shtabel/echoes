﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEmitterController : MonoBehaviour
{

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
    }
}
