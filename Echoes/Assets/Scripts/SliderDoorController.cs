using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SliderDoorController : MonoBehaviour
{
    [SerializeField]
    Vector3 closePosition;    // position of the closed door
    [SerializeField]
    Vector3 openPosition;    // position of the opened door

    [SerializeField]
    float duration;     // duration of activation process

    [SerializeField]
    float delay;        // delay before activation
    
    public bool doorOpened;

    AudioManager am;

    void Start()
    {
        am = FindObjectOfType<AudioManager>();

        DOTween.Init(true, true, LogBehaviour.ErrorsOnly);
        //endDestination += transform.position;        

        //Debug.Log("Pos: " + transform.localPosition);
        //Debug.Log("Closed: " + closePosition);       

        doorOpened = (transform.localPosition == closePosition) ? false : true;
    }

    public void CloseTheDoor()
    {
        am.Play("slider_close");
        //.Log("activate the door");

        Sequence mySequence = DOTween.Sequence();
        mySequence.AppendInterval(delay).Append(transform.DOLocalMove(closePosition, duration));

        doorOpened = false;
    }

    public void OpenTheDoor()
    {
        //Debug.Log("activate the door");

        Sequence mySequence = DOTween.Sequence();
        mySequence.AppendInterval(delay).Append(transform.DOLocalMove(openPosition, duration));

        doorOpened = true;
    }
}
