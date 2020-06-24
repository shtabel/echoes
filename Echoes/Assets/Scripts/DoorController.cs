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

    Vector3 initRotation;

    void Start()
    {
        DOTween.Init(true, true, LogBehaviour.ErrorsOnly);

        initRotation = transform.rotation.eulerAngles;
    }

    public void OpenTheDoor()
    {
        Sequence mySequence = DOTween.Sequence();
        mySequence.AppendInterval(delay).Append(transform.DORotate(endRotation, duration));
    }

    public void CloseTheDoor()
    {
        Sequence mySequence = DOTween.Sequence();
        mySequence.Append(transform.DORotate(initRotation, 1));
    }
}
