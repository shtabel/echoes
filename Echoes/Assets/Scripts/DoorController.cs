using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DoorController : MonoBehaviour
{
    [SerializeField]
    Vector3 endRotation;    // rotation of the opened door

    [SerializeField]
    float duration;     // duration of opening process

    [SerializeField]
    float delay;        // delay before opening

    void Start()
    {
        DOTween.Init(true, true, LogBehaviour.ErrorsOnly);
    }

    public void OpenTheDoor()
    {
        Sequence mySequence = DOTween.Sequence();
        mySequence.AppendInterval(delay).Append(transform.DORotate(endRotation, duration));
    }
}
