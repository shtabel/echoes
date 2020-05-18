using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeaconController : MonoBehaviour
{
    [SerializeField]
    int number;                 // порядковый номер маячка

    [SerializeField]
    float activateDelay;        // задержка в секундах между активациями
    float nextTimeActivate;     // след время активации

    bool ignoreActivation;      // игнорирование активации (когда активен или когда решили пазл)

    [SerializeField]
    Sprite spriteWhite;         // былый спрайт (неактивный)

    [SerializeField]
    Sprite spriteYellow;        // желтый спрайт (активный)

    SpriteRenderer rend;    
    BeaconManager beaconManager;

    

    void Start()
    {
        // устанавливаем маячек в неактивное состояние
        rend = GetComponent<SpriteRenderer>();
        rend.sprite = spriteWhite;
        ignoreActivation = false;

        beaconManager = FindObjectOfType<BeaconManager>();

        nextTimeActivate = Time.time;
    }
    
    public void ActivateBeacon()
    {
        // активируем маячек только по прошествии заланного времени и если он неактивен
        if (Time.time >= nextTimeActivate && !ignoreActivation)
        {
            rend.sprite = spriteYellow;
            ignoreActivation = true;
            // определяем порядок активации
            beaconManager.HandleOrder(number);

            nextTimeActivate = Time.time + activateDelay;           
        }
    }

    public void DeactivateBeacon() // деактивируем маячек
    {
        rend.sprite = spriteWhite;
        ignoreActivation = false;
    }
}
