using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // PUBLIC INIT
    public float thrust;                // приложенная сила  

    //public GameObject radarRay;

    // PRIVATE INIT
    bool isRadarOn;
    float radarRadius = 1.3f;

    GameObject radarRay;

    Vector3 mousePos;   // координаты мыши
    Vector3 direction;  // направление куда смотрит игрок
    
    Rigidbody rb;
    LevelManager lvlManager;
    MenuManager menuManager;
    BlinkManager blinkManager;
    AudioManager audioManager;
    CameraShake camShake;

    void Start()
    {
        isRadarOn = true;

        rb = GetComponent<Rigidbody>();
        var radar = FindObjectOfType<Rotate>();
        radarRay = radar.gameObject;

        camShake = FindObjectOfType<CameraShake>();
        lvlManager = FindObjectOfType<LevelManager>();
        menuManager = FindObjectOfType<MenuManager>();
        blinkManager = FindObjectOfType<BlinkManager>();
        audioManager = FindObjectOfType<AudioManager>();

        direction = Vector3.up;        
    }

    void Update()
    {
        // get mouse position
        mousePos = Input.mousePosition;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);

        FaceMouse(mousePos); // face mouse direction
        
        MovePlayer();

        OtherInput();
    }

    void MovePlayer()
    {
        // считаем расстояние от камеры до игрока, чтоб перемещаться только если курсор внутри ИКО
        float dst = Vector3.Distance(mousePos, transform.position) - Mathf.Abs(Camera.main.transform.position.z);

        // if player controlles with mouse - move towards mouse
        if (Input.GetMouseButton(0) && dst < radarRadius)
        {
            Vector3 force = transform.up * thrust;
            rb.AddForce(force);
            //Debug.Log("Velocity: x = " + rb.velocity.x + "; y = " + rb.velocity.y + "; z = " + rb.velocity.z);
        }
        else if (Input.GetMouseButtonDown(1) && dst < radarRadius)
        {
            transform.position = new Vector3(mousePos.x, mousePos.y, transform.position.z);            
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
        //Debug.Log("Mouse position (player): " + mousePos.x + "; " + mousePos.y);
    }

    void OtherInput()
    {
        if (Input.GetKeyDown(KeyCode.E))    // turn on/off radar
        {
            isRadarOn = !isRadarOn;
            radarRay.SetActive(isRadarOn);
        }

        if (Input.GetKeyDown(KeyCode.Z))    
        {
            camShake.SmallShake();
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            camShake.Shake();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "obstacle")
        {
            camShake.SmallShake();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "end")
        {
            menuManager.LevelCompleted();
            Debug.Log("Level completed!");
        }

        if (other.tag == "mine")
        {
            blinkManager.CreateBlink(blinkManager.mineBlown, other.transform.position);
            Destroy(other.gameObject);

            camShake.Shake();

            DestroyPlayer();
        }

        if (other.tag == "rocket")
        {
            blinkManager.CreateBlink(blinkManager.rocketBlown, other.transform.position);
            Destroy(other.gameObject);

            camShake.Shake();

            DestroyPlayer();
        }

        lvlManager.ResetArrays();
    }

    void DestroyPlayer()
    {
        audioManager.Play("explosion");

        blinkManager.CreateBlink(blinkManager.circleBlown, transform.position);
        MakeVisible(false);     
        menuManager.PlayerDead();
    }

    public void MakeVisible(bool isVisible)
    {
        gameObject.SetActive(isVisible);
        radarRay.SetActive(isVisible);
    }
}
