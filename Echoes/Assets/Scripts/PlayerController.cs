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
    TimerManager timerManager;
    ChatManager chatManager;

    float nextTimeblink;
    float nextTimeblinkSunken;
    [SerializeField]
    float blinkDelay = 0.5f;

    public bool inSafeZone;    // if player in safe zone

    private bool boost = false;
    private float booster = 1f;  // thrust multiplier
    private int boostCd = 0;
    private int boostCdMax = 2000;
    private int boostLen = 0;
    private int boostLenMax = 600;


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
        timerManager = FindObjectOfType<TimerManager>();
        chatManager = FindObjectOfType<ChatManager>();

        direction = Vector3.up;

        nextTimeblink = Time.time;
        nextTimeblinkSunken = Time.time;
    }

    void FixedUpdate()
    {
        // get mouse position
        mousePos = Input.mousePosition;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);

        FaceMouse(mousePos); // face mouse direction
        
        MovePlayer();

        OtherInput();

        Booster();

    }

    void Booster(bool turnON=false){
        booster = boost? 2.5f : 1f; // not optimal but readable and editable
        if (turnON){
            if (!boost && boostCd == 0){
                chatManager.AddText("BOOSTER ACTIVATED!");
                boost = true;
                boostLen = boostLenMax;
            }
        }
        else{
            if (boost){
                if (boostLen > 0)
                    boostLen -=1;
                if (boostLen <= 0){
                    boost = false;
                    boostCd = boostCdMax;
                    chatManager.AddText("BOOSTER DEPLETED!");
                }
            }
            else if (boostCd > 0){
                boostCd -= 1;
                if (boostCd == 0)
                    chatManager.AddText("BOOSTER READY!");
            }
        }
    }

    void MovePlayer()
    {
        float realThrust = thrust * booster;
        // считаем расстояние от камеры до игрока, чтоб перемещаться только если курсор внутри ИКО
        float dst = Vector3.Distance(mousePos, transform.position) - Mathf.Abs(Camera.main.transform.position.z);
        //Debug.Log("dst to click: " + dst);

        // if player controlles with mouse - move towards mouse
        ////if (Input.GetMouseButton(1) && dst < radarRadius)
        ////{
        ////    Vector3 force = transform.up * thrust * 1.5f;
        ////    rb.AddForce(force);
        ////    //Debug.Log("Velocity: x = " + rb.velocity.x + "; y = " + rb.velocity.y + "; z = " + rb.velocity.z);
        ////}
        ////else 
        if (Input.GetMouseButton(0) && dst < radarRadius)
        {
            Vector3 force = transform.up * realThrust;
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
            rb.AddForce(Vector3.ClampMagnitude(force, 1) * realThrust);
        }

        if (Input.GetButtonDown("Jump"))
            Booster(true);
        
    }

    void FaceMouse(Vector3 mousePosition)
    {
        transform.up = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y);
        //Debug.Log("Mouse position (player): " + mousePos.x + "; " + mousePos.y);
    }

    void OtherInput()
    {
#if (UNITY_EDITOR)
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
        if (Input.GetKeyDown(KeyCode.Z))    
        {
            camShake.SmallShake();
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            camShake.Shake();
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            timerManager.StartTimer();
        }
#endif
    }

    void HandleObstacleCollision(Collision collision)
    {
        // если игрок сталкивается с препятстием - покажи точку соприкосновение и потряси
        if ((collision.gameObject.tag == "obstacle" || collision.gameObject.tag == "door"
            || collision.gameObject.tag == "generator") && (Time.time >= nextTimeblink))
        {
            camShake.SmallShake();

            audioManager.Play("wall_hit");

            foreach (ContactPoint contact in collision.contacts)
            {
                if (collision.gameObject.tag == "obstacle")                
                    blinkManager.CreateBlink(blinkManager.blinkGreen, contact.point);
                if (collision.gameObject.tag == "door")
                    blinkManager.CreateBlink(blinkManager.blinkGray, collision.transform.position);

            }

            nextTimeblink = Time.time + blinkDelay;
        }
    }

    void HandleSunkenCollision(Collision collision)
    {
        // если игрок сталкивается с обломком - покажи его
        if (collision.gameObject.tag == "sunken" && (Time.time >= nextTimeblinkSunken))
        {
            blinkManager.CreateBlinkFollow(blinkManager.blinkCircleGray, collision.transform.position, collision.gameObject);
            nextTimeblinkSunken = Time.time + blinkDelay;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        HandleObstacleCollision(collision);
        HandleSunkenCollision(collision);
    }

    void OnCollisionStay(Collision collision)
    {
        HandleObstacleCollision(collision);
        HandleSunkenCollision(collision);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "end")
        {
            menuManager.LevelCompleted();
            //Debug.Log("Level completed!");
        }

        if (other.tag == "mine"|| other.tag == "mine_boss" || other.tag == "rocket" || other.tag == "persuer")
        {
            other.gameObject.GetComponent<EnemyController>().BlowUpEnemy(false);
            
            DestroyPlayer();
        }

        if (other.tag == "safe_zone")
        {
            inSafeZone = true;
        }

        lvlManager.ResetArrays();

        // dark section
        if (other.tag == "dark_start")
        {
            radarRay.SetActive(false);
            if (other.GetComponent<BlowUpMine>() != null)
            {
                audioManager.Play("radar_off");

                other.GetComponent<BlowUpMine>().BlowMine();
                other.gameObject.SetActive(false);
            }
        }
        if (other.tag == "dark_stop")
        {
            audioManager.Play("radar_on");
            radarRay.SetActive(true);
        }

        // boss setion
        if (other.tag == "boss_start")
        {
            // start boss battle
            FindObjectOfType<BossBatleManager>().StartPhase1();
            other.gameObject.SetActive(false);        
        }

        // туннель вслепую
        if (other.tag == "tunnel_start")
        {
            other.gameObject.SetActive(false);
            audioManager.Play("laser_on");
            FindObjectOfType<TunnelEmittersManager>().ActivateEmitters();
        }
        if (other.tag == "tunnel_stop")
        {
            other.gameObject.SetActive(false);
            audioManager.Play("radar_off");
            FindObjectOfType<TunnelEmittersManager>().DeactivateEmitters();
        }

        // finish the game
        if (other.tag == "game_finish")
        {
            //Debug.Log("Game completed!");
            menuManager.GameCompleted();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "safe_zone")
        {
            inSafeZone = false;
        }
    }

    public bool DestroyPlayer()
    {
        if (!inSafeZone)    // if player not in the safe zone - destroy him
        {
            camShake.Shake();
            timerManager.StopTimer();

            audioManager.Play("explosion");

            // deactivate marker
            EndMarkScript endMark = FindObjectOfType<EndMarkScript>();

            if (endMark != null)
            {
                endMark.SetMarker(false, true);
            }


            blinkManager.CreateBlink(blinkManager.blinkCircleOrange, transform.position);
            MakeVisible(false);
            menuManager.PlayerDead();

            return true;
        }

        return false;
    }

    public void MakeVisible(bool isVisible)
    {
        gameObject.SetActive(isVisible);
        radarRay.SetActive(isVisible);
    }
}
