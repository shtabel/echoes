using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generatorv2Controller : MonoBehaviour // генератор, который питает открытую дверь и его нужно запитать
{
    [SerializeField]
    GeneratorManager genMng;
    BlinkManager bm;
    SpriteRenderer rend;

    [SerializeField]
    Sprite spriteWhite;         // былый спрайт (неактивный)

    [SerializeField]
    Sprite spriteYellow;        // желтый спрайт (активный)

    [SerializeField]
    bool activationBeacon;

    bool ignoreActivation;      // игнорирование активации (когда активен или когда решили пазл)

    // Start is called before the first frame update
    void Start()
    {
        bm = FindObjectOfType<BlinkManager>();

        rend = GetComponent<SpriteRenderer>();
        rend.sprite = spriteWhite;

        ignoreActivation = false;
    }

    public void ActivateGenerator()
    {
        if (!ignoreActivation)
        {
            rend.sprite = spriteYellow;

            genMng.MinusGenerator();

            ignoreActivation = true;

            //Debug.Log("genertor activated");
        }
        
    }

    public void DeactivateGenerator()
    {
        if (ignoreActivation)
        {
            rend.sprite = spriteWhite;

            genMng.PlusGenerator();

            ignoreActivation = false;

            //Debug.Log("genertor deactivated");
        }
        
    }
}
