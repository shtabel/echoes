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

    public GameObject blink;        // элемент блинк
    public GameObject mine;         // мина
    public GameObject rocket;         // ракета
    public GameObject detectionBlink;   // объект который указывает точку назначения ракеты
    public GameObject mineBlown;        // взорванная мина
    public GameObject rocketBlown;      // взорванная ракета

    // PRIVATE INIT


    // Start is called before the first frame update
    void Start()
    {    
        // creating pools
        // create pool of blinks
        PoolManager.instance.CreatePool(blink, poolOfBlinks);
        // create pool of mines
        PoolManager.instance.CreatePool(mine, poolOfMines);
        // create pool of rockets
        PoolManager.instance.CreatePool(rocket, poolOfRockets);
        // create pool of detection blinks
        PoolManager.instance.CreatePool(detectionBlink, poolOfDetectionBlinks);
        // create pool of blown mines
        PoolManager.instance.CreatePool(mineBlown, poolOfMinesBlown);
        // create pool of blown rockets
        PoolManager.instance.CreatePool(rocketBlown, poolOfRocketsBlown);

    }

    public void CreateBlink(GameObject blinkType, Vector3 position)
    {
        PoolManager.instance.ReuseObject(blinkType, position, Quaternion.Euler(0, 0, 0));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
