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
        //ignoreActivation = false;

        //initType = activationBeacon;

        // for the blinking animation
        spriteColor = rend.color;
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