using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    // PUBLIC INIT


    // PRIVATE INIT
    GameObject[] arrObstacles;  // массив препядствий
    GameObject[] arrMines;      // массив мин
    GameObject[] arrRockets;    // массив ракет

    bool objectsVisible;        // видно/не видно объекты


    // Start is called before the first frame update
    void Start()
    {
        VisibleObjects(false);
        
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
    }    

    public void ResetArrays()
    {
        arrMines = null;
        arrRockets = null;
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
