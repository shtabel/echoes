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

    // for the blinking animation
    Color spriteColor;
    bool fading = false;

    float duration = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        // устанавливаем маячек в неактивное состояние
        rend = GetComponent<SpriteRenderer>();
        rend.sprite = spriteNotActive;
        ignoreActivation = false;

        //beaconManager = FindObjectOfType<Beaconv2Manager>();

        nextTimeActivate = Time.time;

        // for the blinking animation
        spriteColor = rend.color;
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

    public void Fade(bool fadeIn)
    {
        if (fading)
        {
            return;
        }
        fading = false;

        StartCoroutine(FadeTo(fadeIn, duration));
    }


    IEnumerator FadeTo(bool fadeIn, float duration)
    {
        float counter = 0f;

        //Set Values depending on if fadeIn or fadeOut
        float a, b;
        if (fadeIn)
        {
            a = 0;
            b = 1;
        }
        else
        {
            a = 1;
            b = 0;
        }

        //Enable MyRenderer component
        if (!rend.enabled)
            rend.enabled = true;

        //Do the actual fading
        while (counter < duration)
        {
            counter += Time.deltaTime;
            float alpha = Mathf.Lerp(a, b, counter / duration);
            //Debug.Log(alpha);

            rend.color = new Color(spriteColor.r, spriteColor.g, spriteColor.b, alpha);
            yield return null;
        }

        if (!fadeIn)
        {
            //Disable Mesh Renderer
            rend.enabled = false;

        }
        fading = false; //So that we can call this function next time

        StartCoroutine(FadeTo(!fadeIn, duration));
    }
}
