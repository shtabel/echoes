using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RocketSpawner : MonoBehaviour
{
    [SerializeField]
    GameObject rocketPrefab;

    [SerializeField]
    float launchForce;
    [SerializeField]
    float lauchDelay;

    [SerializeField]
    bool startWithRocket;

    void Start()
    {
        if (startWithRocket)
            SpawnRocket();
    }

    public void SpawnRocket()
    {
        //Debug.Log("Spawn Rocket");
        Instantiate(rocketPrefab, transform.position, Quaternion.identity);
    }

    public void LaunchRocket()
    {
        GameObject rocket = null;

        // wait lauchDelay-seconds, then instantiate a rocket and launch it
        Sequence s = DOTween.Sequence();
        s.AppendInterval(lauchDelay).AppendCallback(() => rocket = Instantiate(rocketPrefab, transform.position, Quaternion.identity));
        s.AppendCallback(() => rocket.GetComponent<Rigidbody>().AddForce(transform.up * launchForce));


        //rocket.GetComponent<Rigidbody>().AddForce(transform.up * launchForce);
    }
}
