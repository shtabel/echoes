using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    // PUBLIC INIT
    public float rotationDegree;

    // private init
    Vector3 direction;    

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0.0f, 0.0f, -rotationDegree * Time.deltaTime);

        Raycast();
    }

    public void Raycast()
    {
        Vector3 upVec = transform.TransformDirection(Vector3.up);

        Ray ray = new Ray(transform.position, upVec);
        Debug.DrawRay(transform.position, upVec * 5, Color.green);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1))
        {
            print(hit.collider.name);
            Debug.Log(hit.collider.name);
        }
    }
}
