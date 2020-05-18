﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeaconManager : MonoBehaviour
{
    [SerializeField]
    BeaconController[] beacons; // массив маячков
    
    int lastBeaconActivated;    // номер последнего активированного маячка
    int counter;                // счетчик активированных маячков


    // Start is called before the first frame update
    void Start()
    {
        Reset();
    }

    public void HandleOrder(int beaconNumber)
    {
        // если активировали маячек в нужной последовательности
        if (beaconNumber == lastBeaconActivated + 1)
        {
            if (beaconNumber == 1) //  если это первый - то сбрасываем все остальные
            {
                DeactivateBeacons(beaconNumber);
            }

            //  ведем счет и изадаем след элемент в последовательности
            counter++;
            lastBeaconActivated++;

            //  проверка активации всех маячков
            if (counter == beacons.Length)
            {
                PuzzleSolved();
            }
        }
        //  если активировали маячек не в нужной последовательности - сброс прогресса
        else
        {
            Reset();
            DeactivateBeacons(beaconNumber);
        }
    }

    void PuzzleSolved()
    {
        //Debug.Log("Quest Solved!");

        // OPEN THE DOOR
    }

    void Reset()    // обнуляем прогресс
    {
        lastBeaconActivated = 0;
        counter = 0;
    }

    void DeactivateBeacons(int beaconNumber) // деактивирем все блинки кроме последнего активного
    {
        for (int i = 0; i < beacons.Length; i++)
        {
            if ((i+1) != beaconNumber)
            {
                beacons[i].DeactivateBeacon();
            }
        }
    }
}
