using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beaconv2Controller : MonoBehaviour
{
    public bool activationBeacon;   // true (!) - активируем для открытия двери
                                    // false (x) - деактивирует все активные маячки

    public int id;  // id of the beacon

    [SerializeField]
    float activateDelay;        // задержка в секундах между активациями
    float nextTimeActivate;     // след время активации

    bool ignoreActivation;      // игнорирование активации (когда активен или когда решили пазл)

    [SerializeField]
    Sprite spriteNotActive;
    [SerializeField]
    Sprite spriteActive;

    SpriteRenderer rend;
    [SerializeField]
    Beaconv2Manager beaconManager;

    // Start is called before the first frame update
    void Start()
    {
        // устанавливаем маячек в неактивное состояние
        rend = GetComponent<SpriteRenderer>();
        rend.sprite = spriteNotActive;
        ignoreActivation = false;

        //beaconManager = FindObjectOfType<Beaconv2Manager>();

        nextTimeActivate = Time.time;
    }
    
    public void ActivateBeacon()
    {
        // активируем маячек только по прошествии заланного времени и если он неактивен
        if (Time.time >= nextTimeActivate && !ignoreActivation)
        {
            if (!beaconManager.puzzleSolved)
            {
                rend.sprite = spriteActive;
                ignoreActivation = true;
                // определяем порядок активации
                beaconManager.HandleActivation(id);

                nextTimeActivate = Time.time + activateDelay;
            }           
        }
    }

    public void DeactivateBeacon()
    {
        rend.sprite = spriteNotActive;
        ignoreActivation = false;
    }
}
