using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyWithDelay : MonoBehaviour
{
    // PUBLIC INIT
    public float lifeTime;

    // PRIVATE INIT
    bool fading = false;
    MeshRenderer rend;
    Color currentColor = Color.clear;

    void Start()
    {
        rend = GetComponent<MeshRenderer>();

        //StartCoroutine("FadeTo");
        // currentColor = rend.material.color;

        Fade(false, 2f);//Fade Out
    }

    IEnumerator FadeTo(bool fadeIn, float duration)
    {
        //MyRenderer.material.
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

        //Get original Mesh Color
        Color meshColor = rend.material.color;


        //Do the actual fading
        while (counter < duration)
        {
            counter += Time.deltaTime;
            float alpha = Mathf.Lerp(a, b, counter / duration);
            Debug.Log(alpha);

            rend.material.color = new Color(meshColor.r, meshColor.g, meshColor.b, alpha);
            yield return null;
        }

        if (!fadeIn)
        {
            //Disable Mesh Renderer
            rend.enabled = false;
        }
        fading = false; //So that we can call this function next time
    }


    void Update()
    {
        //Destroy(gameObject, lifeTime);

        //Color color = rend.material.color; 
        //color.a = alpha;
        //rend.material.color = new Color(currentColor.r, currentColor.g, currentColor.b, alpha);
        //Debug.Log(alpha);
    }

    void Fade(bool fadeIn, float duration)
    {
        if (fading)
        {
            return;
        }
        fading = true;

        changeModeToFade();
        StartCoroutine(FadeTo(fadeIn, duration));
    }

    void changeModeToFade()
    {
        rend.material.SetFloat("_Mode", 2);
        rend.material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        rend.material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        rend.material.SetInt("_ZWrite", 0);
        rend.material.DisableKeyword("_ALPHATEST_ON");
        rend.material.EnableKeyword("_ALPHABLEND_ON");
        rend.material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        rend.material.renderQueue = 3000;
    }

    // Define an enumerator to perform our fading.
    // Pass it the material to fade, the opacity to fade to (0 = transparent, 1 = opaque),
    // and the number of seconds to fade over.
    //IEnumerator FadeTo()
    //{
    //    // Cache the current color of the material, and its initiql opacity.
    //    Color color = mat.color;
    //    float startOpacity = color.a;

    //    // Track how many seconds we've been fading.
    //    float t = 0;

    //    while (t < lifeTime)
    //    {
    //        // Step the fade forward one frame.
    //        t += Time.deltaTime;
    //        // Turn the time into an interpolation factor between 0 and 1.
    //        float blend = Mathf.Clamp01(t / lifeTime);

    //        // Blend to the corresponding opacity between start & target.
    //        color.a = Mathf.Lerp(startOpacity, 0f, blend);

    //        // Apply the resulting color to the material.
    //        mat.color = color;

    //        // Wait one frame, and repeat.
    //        yield return null;
    //    }
    //}
}
