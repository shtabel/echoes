using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeWithDelay : PoolObject
{
    // PUBLIC INIT
    public float lifeTime;

    // PRIVATE INIT
    bool fading = false;
    MeshRenderer rend;
    Color currentColor = Color.clear;
    Color meshColor;

    void Start()
    {
        rend = GetComponent<MeshRenderer>(); 

        meshColor = rend.material.color;
        rend.material.color = new Color(meshColor.r, meshColor.g, meshColor.b, 0);

        if (gameObject.tag == "mine")   // если это мина (она не в пуле), то сразу запускаем fading 
        {
            Fade(false, lifeTime);//Fade Out
        }
        
        
    }

    public override void OnObjectReuse()
    {
        //transform.localScale = Vector3.one;
        rend.enabled = true;
        rend.material.color = new Color(meshColor.r, meshColor.g, meshColor.b, 1);

        Fade(false, lifeTime);
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
        //Color meshColor = rend.material.color;

        //Do the actual fading
        while (counter < duration)
        {
            counter += Time.deltaTime;
            float alpha = Mathf.Lerp(a, b, counter / duration);
            //Debug.Log(alpha);

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
}
