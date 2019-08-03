using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // PUBLIC INIT
    public float thrust;                // приложенная сила  

    public GameObject radarRay;

    // PRIVATE INIT
    Vector3 mousePos;   // координаты мыши
    Vector3 direction;  // направление куда смотрит игрок
    
    Rigidbody rb;
    LevelManager lvlManager;
    MenuManager menuManager;
    BlinkManager bm;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        lvlManager = FindObjectOfType<LevelManager>();
        menuManager = FindObjectOfType<MenuManager>();
        bm = FindObjectOfType<BlinkManager>();

        direction = Vector3.up;        
    }

    void Update()
    {
        // get mouse position
        mousePos = Input.mousePosition;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);

        FaceMouse(mousePos); // face mouse direction
        
        MovePlayer();
    }

    void MovePlayer()
    {
        // считаем расстояние от камеры до игрока, чтоб перемещаться только если курсор внутри ИКО
        float dst = Vector3.Distance(mousePos, transform.position) - Mathf.Abs(Camera.main.transform.position.z);

        // if player controlles with mouse - move towards mouse
        if (Input.GetMouseButton(0) && dst < 1.3)
        {
            Vector3 force = transform.up * thrust;
            rb.AddForce(force);
            //Debug.Log("Velocity: x = " + rb.velocity.x + "; y = " + rb.velocity.y + "; z = " + rb.velocity.z);
        }
        // if player controlles with keyboard - move according to keuboard
        else if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        {
            float inputX = Input.GetAxisRaw("Horizontal");
            float inputY = Input.GetAxisRaw("Vertical");
            Vector3 force = new Vector3(inputX, inputY, 0f);
            rb.AddForce(force * thrust);
        }
        
    }

    void FaceMouse(Vector3 mousePosition)
    {
        transform.up = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "end")
        {
            menuManager.LevelCompleted();
        }

        if (other.tag == "mine")
        {
            bm.CreateBlink(bm.mineBlown, other.transform.position);
            DestroyPlayer(other);
        }

        if (other.tag == "rocket")
        {
            bm.CreateBlink(bm.rocketBlown, other.transform.position);
            DestroyPlayer(other);
        }

        lvlManager.ResetArrays();
    }

    void DestroyPlayer(Collider col)
    {
        bm.CreateBlink(bm.mineBlown, transform.position);
        Destroy(col.gameObject);

        radarRay.SetActive(false);

        menuManager.PlayerDead();
        gameObject.SetActive(false);
    }
}
