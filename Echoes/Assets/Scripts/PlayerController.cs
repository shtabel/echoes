using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // PUBLIC INIT
    public float duration;              // время на разгон
    public Vector2 startMaxSpeed;       // стартовая и максимальная скорости

    public GameObject startMenu;        // ссылка на start menu
    public GameObject endMenu;          // ссылка на end menu


    // PRIVATE INIT
    float speed;        // скорость в настоящий момент
    Vector3 mousePos;   // координаты мыши
    Vector3 direction;  // направление куда смотрит игрок

    Vector3 velocity;   // скорость
        
    GameObject[] arrObstacles;  // массив препядствий
    GameObject[] arrMines;      // массив мин
    bool objectsVisible;        // видно/не видно объекты

    void Start()
    {
        direction = Vector3.up;
        
        VisibleObjects(false);

        startMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    void Update()
    {
        // get mouse position
        mousePos = Input.mousePosition;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);

        FaceMouse(mousePos); // face mouse direction

        HandleInput();

        MovePlayer();
    }

    void MovePlayer()
    {
        // считаем расстояние от камеры до игрока, чтоб перемещаться только если курсор внутри ИКО
        float dst = Vector3.Distance(mousePos, transform.position) - Mathf.Abs(Camera.main.transform.position.z);

        // if player controlles with mouse - move towards mouse
        if (Input.GetMouseButton(0) && dst < 1.3)
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

    void HandleInput()
    {
        // make obstacles visible/invisible
        if (Input.GetKeyDown(KeyCode.V))    
        {
            VisibleObjects(!objectsVisible);
            //Debug.Log("V is pressed");
        }


    }

    void VisibleObjects(bool makeVisible)
    {
        if (arrObstacles == null)
            arrObstacles = GameObject.FindGameObjectsWithTag("obstacle");

        if (arrMines == null)
            arrMines = GameObject.FindGameObjectsWithTag("mine");

        objectsVisible = makeVisible;

        foreach (GameObject obstacle in arrObstacles)
        {
            obstacle.GetComponent<MeshRenderer>().enabled = objectsVisible;
        }

        foreach (GameObject mine in arrMines)
        {
            mine.GetComponent<MeshRenderer>().enabled = objectsVisible;
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
            Debug.Log("Level completed!");
            endMenu.SetActive(true);
            Time.timeScale = 0f;
        }
        
    }
}
