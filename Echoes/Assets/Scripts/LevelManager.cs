using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    // PUBLIC INIT
    public GameObject startMenu;        // ссылка на start menu
    public GameObject endMenu;          // ссылка на end menu

    // PRIVATE INIT
    GameObject[] arrObstacles;  // массив препядствий
    GameObject[] arrMines;      // массив мин
    GameObject[] arrRockets;    // массив ракет

    bool objectsVisible;        // видно/не видно объекты


    // Start is called before the first frame update
    void Start()
    {
        VisibleObjects(false);

        startMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
    }

    public void LevelCompleted()
    {
        Debug.Log("Level completed!");

        endMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    void VisibleObjects(bool makeVisible)
    {
        if (arrObstacles == null)
            arrObstacles = GameObject.FindGameObjectsWithTag("obstacle");

        if (arrMines == null)
            arrMines = GameObject.FindGameObjectsWithTag("mine");

        if (arrRockets == null)
            arrRockets = GameObject.FindGameObjectsWithTag("rocket");

        objectsVisible = makeVisible;

        SetVisible(arrObstacles);
        SetVisible(arrMines);
        SetVisible(arrRockets);
    }

    void SetVisible(GameObject[] array)
    {
        foreach (GameObject obj in array)
        {
            obj.GetComponent<MeshRenderer>().enabled = objectsVisible;
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
}
