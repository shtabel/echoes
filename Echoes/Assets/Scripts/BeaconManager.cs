using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeaconManager : MonoBehaviour
{
    [SerializeField]
    BeaconController[] beacons; // массив маячков
    
    int lastBeaconActivated;    // номер последнего активированного маячка
    int counter;                // счетчик активированных маячков

    //[SerializeField]
    //DoorController theDoor;

    [SerializeField]
    SliderDoorController[] sliderDoors;

    [SerializeField]
    GameObject emitterBase;
    [SerializeField]
    GameObject emitter;

    // Start is called before the first frame update
    void Start()
    {
        Reset();

        //if (FindObjectOfType<SaveManager>().CheckRightPuzzleSolved())
        //{
        //    PuzzleSolved();
        //}
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

    public void PuzzleSolved()
    {
        //Debug.Log("Quest Solved!");

        BeaconBlink();
        // OPEN THE DOOR
        //theDoor.OpenTheDoor();
        for (int i = 0; i < sliderDoors.Length; i++)
        {
            sliderDoors[i].OpenTheDoor(true);
        }

        // отключаем эмиттер
        if (emitterBase != null)
            Instantiate(emitterBase, emitter.transform.position, emitter.transform.rotation);
        if (emitter != null)
            emitter.SetActive(false);

        FindObjectOfType<SaveManager>().SetRightPuzzleSolved(1);
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

    void BeaconBlink()
    {
        for (int i = 0; i < beacons.Length; i++)
        {            
            beacons[i].Fade(false);
        }
    }
}
