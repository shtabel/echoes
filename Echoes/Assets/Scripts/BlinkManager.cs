using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkManager : MonoBehaviour
{
    // PUBLIC INIT
    // Пулы
    public int poolOfBlinks;        // количество блинков в пуле
    public int poolOfMines;        // количество мин в пуле
    public int poolOfRockets;        // количество ракет в пуле
    public int poolOfDetectionBlinks;        // количество детекционных блинков в пуле
    public int poolOfMinesBlown;        // количество взорванных мин в пуле
    public int poolOfRocketsBlown;        // количество взорванных ракет в пуле
    public int poolOfCircleBlown;        // количество взорванных кругов в пуле
    public int poolOfPersuers;      // количество преследователей в пуле
    public int poolOfRunaways;      // количество беглецов в пуле
    public int poolOfSearchersBlown;    // количество взорванных поисковиков в пуле
    public int poolOfSunkens;
    public int poolOfBlueBlinks;

    public GameObject blinkGreen;        // элемент блинк
    public GameObject blinkCrossRed;         // мина
    public GameObject blinkTriangleRed;         // ракета
    public GameObject blinkRed;   // объект который указывает точку назначения ракеты
    public GameObject blinkCrossOrange;        // взорванная мина
    public GameObject blinkTriangleOrange;      // взорванная ракета
    public GameObject blinkCircleOrange;      // взорванный игрок
    public GameObject blinkCircleRed;
    public GameObject blinkCirclePink;
    public GameObject blinkSquareOrange;
    public GameObject blinkCircleGray;
    public GameObject blinkGray;


    public float blinkLifeTimeShort;   // время быстрого угасания блинков
    public float blinkLifeTimeLong;    // время временного угасания 

    // PRIVATE INIT


    // Start is called before the first frame update
    void Start()
    {    
        // creating pools
        // create pool of blinks
        PoolManager.instance.CreatePool(blinkGreen, poolOfBlinks);
        // create pool of mines
        PoolManager.instance.CreatePool(blinkCrossRed, poolOfMines);
        // create pool of rockets
        PoolManager.instance.CreatePool(blinkTriangleRed, poolOfRockets);
        // create pool of detection blinks
        PoolManager.instance.CreatePool(blinkRed, poolOfDetectionBlinks);
        // create pool of blown mines
        PoolManager.instance.CreatePool(blinkCrossOrange, poolOfMinesBlown);
        // create pool of blown rockets
        PoolManager.instance.CreatePool(blinkTriangleOrange, poolOfRocketsBlown);
        // create pool of blown rockets
        PoolManager.instance.CreatePool(blinkCircleOrange, poolOfCircleBlown);
        // create pool of persuers
        PoolManager.instance.CreatePool(blinkCircleRed, poolOfPersuers);
        // create pool of runaways
        PoolManager.instance.CreatePool(blinkCirclePink, poolOfRunaways);
        // create pool of blown searchers
        PoolManager.instance.CreatePool(blinkSquareOrange, poolOfSearchersBlown);
        // create pool of gray circles
        PoolManager.instance.CreatePool(blinkCircleGray, poolOfSunkens);
        // create pool of blue blinks
        PoolManager.instance.CreatePool(blinkGray, poolOfBlueBlinks);
    }

    public void CreateBlink(GameObject blinkType, Vector3 position)
    {
        PoolManager.instance.ReuseObject(blinkType, position, Quaternion.Euler(0, 0, 0));
    }

    public void CreateBlinkFollow(GameObject blinkType, Vector3 position, GameObject tempParent)
    {
        PoolManager.instance.ReuseObjectFollow(blinkType, position, Quaternion.Euler(0, 0, 0), tempParent);
    }

}
