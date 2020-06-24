using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BBeaconEmitterManager : MonoBehaviour
{
    [SerializeField]
    BBeaconEmitterController[] beacons; // массив маячков

    int requiredAmmount;    // сколько нужно активировать маячков для открытия двери
    int counter;                    // счетчик активированных маячков

    [SerializeField]
    DoorController theDoor;

    bool puzzleSolved;

    void Start()
    {
        counter = 0;
        requiredAmmount = CountActivationBeacons(beacons);
    }

    int CountActivationBeacons(BBeaconEmitterController[] bb)
    {
        int numberOfActivationBeacons = 0;

        for (int i = 0; i < bb.Length; i++)
        {
            if (bb[i].activationBeacon)
            {
                numberOfActivationBeacons++;
            }
        }

        return numberOfActivationBeacons;
    }
    
    public void ActivateBeacon(bool isActivationBeacon)
    {
        if (!puzzleSolved)
        {
            if (!isActivationBeacon)
            {
                // substruct counter
                counter--;
            }
            else if (isActivationBeacon) // если это активационный маячек (!)
            {
                counter++;
                // check puzzle solved
                CheckPuzzleSolved();
            }            
        }        

    }

    void CheckPuzzleSolved()
    {
        //if (counter != requiredAmmount && !puzzleSolved)
        //{
        //    // keep the door closed
        //    Debug.Log("Close the door");
        //    theDoor.CloseTheDoor();

        //}
        //else 
        if (counter == requiredAmmount && !puzzleSolved)
        {
            // open the door
            //Debug.Log("Open the door");
            theDoor.OpenTheDoor();

            puzzleSolved = true;
        }
    }

}
