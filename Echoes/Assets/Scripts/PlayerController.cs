using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // PUBLIC INIT
    public float thrust;                // приложенная сила  

    public Rigidbody rb;


    // PRIVATE INIT
    bool isRadarOn;
    [SerializeField]
    float radarRadius;

    GameObject radarRay;

    Vector3 mousePos;   // координаты мыши
    Vector3 direction;  // направление куда смотрит игрок

    // references
    CameraShake camShake;

    // managers
    LevelManager lvlManager;
    MenuManager menuManager;
    BlinkManager blinkManager;
    AudioManager audioManager;
    

    void Start()
    {
        isRadarOn = true;

        //rb = GetComponent<Rigidbody>();
        var radar = FindObjectOfType<Rotate>();
        radarRay = radar.gameObject;

        camShake = FindObjectOfType<CameraShake>();
        lvlManager = FindObjectOfType<LevelManager>();
        menuManager = FindObjectOfType<MenuManager>();
        blinkManager = FindObjectOfType<BlinkManager>();
        audioManager = FindObjectOfType<AudioManager>();

        direction = Vector3.up;        
    }

    void FixedUpdate()
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
        //Debug.Log("dst to click: " + dst);

        // if player controlles with mouse - move towards mouse
        if (Input.GetMouseButton(1) && dst < radarRadius)
        {
            Vector3 force = transform.up * thrust * 1.5f;
            rb.AddForce(force);
            //Debug.Log("Velocity: x = " + rb.velocity.x + "; y = " + rb.velocity.y + "; z = " + rb.velocity.z);
        }
        else if (Input.GetMouseButton(0) && dst < radarRadius)
        {
            Vector3 force = transform.up * thrust;
            rb.AddForce(force) ;
            //Debug.Log("Velocity: x = " + rb.velocity.x + "; y = " + rb.velocity.y + "; z = " + rb.velocity.z);
        }
#if (UNITY_EDITOR)
        //else if (Input.GetMouseButtonDown(1) && dst < radarRadius)
        //{
        //    transform.position = new Vector3(mousePos.x, mousePos.y, transform.position.z);            
        //}
#endif
        // if player controlles with keyboard - move according to keuboard
        else if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        {
            float inputX = Input.GetAxisRaw("Horizontal");
            float inputY = Input.GetAxisRaw("Vertical");
            Vector3 force = new Vector3(inputX, inputY, 0f);

            rb.AddForce(Vector3.ClampMagnitude(force, 1) * thrust);
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

        if (Input.GetKeyDown(KeyCode.N))
        {
            menuManager.LevelCompleted();
        }

        // проверка трясущейся камеры: слабое и сильное трясение
#if (UNITY_EDITOR)

        if (Input.GetKeyDown(KeyCode.Z))    
        {
            camShake.SmallShake();
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            camShake.Shake();
        }
#endif
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
            other.gameObject.GetComponent<EnemyController>().BlowUpMine();
            
            DestroyPlayer();
        }
        if (other.tag == "rocket")
        {
            other.gameObject.GetComponent<RocketController>().BlowUpRocket();
            
            DestroyPlayer();
        }
        if (other.tag == "persuer")
        {
            other.gameObject.GetComponent<PersuerController>().BlowUpPersuer();
            
            DestroyPlayer();
        }

        lvlManager.ResetArrays();
    }

    public void DestroyPlayer()
    {
        camShake.Shake();
        audioManager.Play("explosion");
        
        // deactivate marker
        EndMarkScript endMark = FindObjectOfType<EndMarkScript>();

        if (endMark != null)
        {
            endMark.SetMarker(false, true);
        }
        

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
