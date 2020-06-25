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

    // for the blinking animation
    Color spriteColor;
    bool fading = false;

    float duration = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        bm = FindObjectOfType<BlinkManager>();

        rend = GetComponent<SpriteRenderer>();
        rend.sprite = spriteWhite;

        ignoreActivation = false;

        // for the blinking animation
        spriteColor = rend.color;
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
