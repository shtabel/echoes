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
    GameObject[] arrPersuers;   // массив преследователей
    GameObject[] arrRunaways;   // массив беш=глецов
    GameObject[] arrSunkens;   // массив беглецов
    GameObject[] arrDoors;   // массив беш=глецов

    [SerializeField]
    GameObject radarForeground;
    [SerializeField]
    GameObject radarLayout;

    bool objectsVisible;        // видно/не видно объекты
    bool radarVisible = true;   // видно/не видно перед радара

    // Start is called before the first frame update
    void Start()
    {
        VisibleObjects(false);
        VisibleRadar(radarVisible);
        radarLayout.SetActive(false);
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
        arrPersuers = null;
        arrRunaways = null;
        arrSunkens = null;
        arrDoors = null;
    }

    void VisibleObjects(bool makeVisible)
    {
        if (arrObstacles == null)
            arrObstacles = GameObject.FindGameObjectsWithTag("obstacle");

        if (arrMines == null)
            arrMines = GameObject.FindGameObjectsWithTag("mine");

        if (arrRockets == null)
            arrRockets = GameObject.FindGameObjectsWithTag("rocket");

        if (arrPersuers == null)
            arrPersuers = GameObject.FindGameObjectsWithTag("persuer");

        if (arrRunaways == null)
            arrRunaways = GameObject.FindGameObjectsWithTag("runaway");

        if (arrSunkens == null)
            arrSunkens = GameObject.FindGameObjectsWithTag("sunken");

        if (arrDoors == null)
            arrDoors = GameObject.FindGameObjectsWithTag("door");

        objectsVisible = makeVisible;

        SetVisible(arrObstacles);
        SetVisible(arrMines);
        SetVisible(arrRockets);
        SetVisible(arrPersuers);
        SetVisible(arrRunaways);
        SetVisible(arrSunkens);
        SetVisible(arrDoors);
    }

    void VisibleRadar(bool isVisible)
    {
        radarForeground.SetActive(isVisible);
        radarVisible = isVisible;
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
#if (UNITY_EDITOR)

        // make obstacles visible/invisible
        if (Input.GetKeyDown(KeyCode.V))
        {
            VisibleObjects(!objectsVisible);
            //Debug.Log("V is pressed");
        }

        // make obstacles visible/invisible
        if (Input.GetKeyDown(KeyCode.F))
        {
            VisibleRadar(!radarVisible);
        }
#endif
    }
}
