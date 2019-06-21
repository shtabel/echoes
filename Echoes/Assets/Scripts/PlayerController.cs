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

    Vector3 velocity;

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

        // if player controlles with mouse - move towards mouse
        if (Input.GetMouseButton(0)) 
        {
            speed += startMaxSpeed.y * Time.deltaTime;
            speed = Mathf.Clamp(speed, startMaxSpeed.x, startMaxSpeed.y);

            transform.position += transform.up * Time.deltaTime * speed;
        }
        // if player controlles with keyboard - move according to keuboard
        else if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        {
            speed += startMaxSpeed.y * Time.deltaTime;
            speed = Mathf.Clamp(speed, startMaxSpeed.x, startMaxSpeed.y);

            velocity = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0).normalized * speed;
            transform.position += velocity * Time.deltaTime * speed;
        }
        // if not moving
        else
        {
            speed = startMaxSpeed.x;
            velocity = new Vector3(0, 0, 0);
        }
    }

    void FaceMouse(Vector3 mousePosition)
    {
        transform.up = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y);
    }
}
