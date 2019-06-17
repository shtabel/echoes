using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    // PUBLIC INIT
    public float rotationDegree;    // величина в градусах на которую вращается радар
    public float rayLength;         // длина луча
    public float blinkDelay;        // задержка перед появлением следующего блинка

    public GameObject blink;        // элемент блинк
    public GameObject mine;         // мина

    public LayerMask obstacleMask;  // маска преград
    public LayerMask mineMask;      // маска мин

    // private init
    float[] nextTimeBlink = new float[2];            // время след блинка (препятствия, мина)
      

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))    // change rotation direction
        {
            rotationDegree = rotationDegree * -1;
        }

        // rotate ray
        transform.Rotate(0.0f, 0.0f, -rotationDegree * Time.deltaTime);

        Raycast();  // cast ray
    }

    public void Raycast()
    {
        Vector3 upVec = transform.TransformDirection(Vector3.up);

        // draw ray in scene window
        Debug.DrawRay(transform.position, upVec * rayLength, Color.green);
        // cast ray
        RaycastHit hit;

        // if hit obstacle
        if (Physics.Raycast(transform.position, upVec, out hit, rayLength, obstacleMask) && Time.time > nextTimeBlink[0])
        {
            //Debug.Log("hit obstacle");
            Instantiate(blink, hit.point, Quaternion.Euler(0, 0, 0));

            nextTimeBlink[0] = Time.time + blinkDelay;
        }

        // if hit mine
        if (Physics.Raycast(transform.position, upVec, out hit, rayLength, mineMask) && Time.time > nextTimeBlink[1])
        {
            float dstToTarget = Vector3.Distance(transform.position, hit.point);
            if (!Physics.Raycast(transform.position, upVec, dstToTarget, obstacleMask)) // если препятствие не перекрывает
            {
                //Debug.Log("hit mine");
                Instantiate(mine, hit.point, Quaternion.Euler(0, 0, 0));

                nextTimeBlink[1] = Time.time + blinkDelay * 2f;
            }           
        }
    }
}
