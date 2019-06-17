using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // PUBLIC INIT
    public float duration;
    public Vector2 startMaxSpeed;


    // PRIVATE INIT
    float speed;
    Vector3 mousePos;
    Vector3 direction;
    
    void Start()
    {
        direction = Vector3.up;
    }
    
    void Update()
    {
        // get mouse position
        mousePos = Input.mousePosition;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);

        FaceMouse(mousePos); // face mouse direction

        if (Input.GetMouseButton(0)) // move towards mouse
        {
            speed += startMaxSpeed.y * Time.deltaTime;
            speed = Mathf.Clamp(speed, startMaxSpeed.x, startMaxSpeed.y);

            transform.position += transform.up * Time.deltaTime * speed;
        } else
        {
            speed = startMaxSpeed.x;
        }

    }

    void FaceMouse(Vector3 mousePosition)
    {
        transform.up = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y);
    }
}
