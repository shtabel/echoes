using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BBeaconEmitterController : MonoBehaviour
{
    public bool activationBeacon;   // true (!) - активируем для открытия двери
                                    // false (x) - деактивирует все активные маячки

    [SerializeField]                // sprites of active and inactive beacon
    Sprite spriteNotActive;
    [SerializeField]
    Sprite spriteActive;

    [SerializeField]
    BBeaconEmitterManager bm;   // beacon manager

    SpriteRenderer rend;        // sprite renderer

    public bool isActive;

    // Start is called before the first frame update
    void Start()
    {
        // устанавливаем маячек в неактивное состояние
        rend = GetComponent<SpriteRenderer>();
        rend.sprite = spriteNotActive;
        //ignoreActivation = false;

        //initType = activationBeacon;
    }

    public void Activate()  // activate beacon
    {
        rend.sprite = spriteActive;
        bm.ActivateBeacon(activationBeacon);
        isActive = true;
    }

    public void Deactivate()
    {
        rend.sprite = spriteNotActive;
        bm.ActivateBeacon(!activationBeacon);
        isActive = false;
    }
}