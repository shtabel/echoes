using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderDoorTrigger : MonoBehaviour
{
    [SerializeField]
    SliderDoorController sliderDoor;

    [SerializeField]
    bool toOpen;                        // открываем или закрываем дверь

    [SerializeField]
    string tagOfProvocator;             // кто активирует триггер

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == tagOfProvocator && tag == "trigger")
        {
            if (toOpen && !sliderDoor.doorOpened)   // если триггер открывает дверь и дверь закрыта
            {
                sliderDoor.OpenTheDoor(true);
                gameObject.SetActive(false);
            }               
            else if (!toOpen && sliderDoor.doorOpened) // если закрываем дверь и она открыта
            {
                sliderDoor.CloseTheDoor(true);
                gameObject.SetActive(false);
            }
        }
    }
}
