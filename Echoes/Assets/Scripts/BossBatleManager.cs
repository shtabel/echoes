using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBatleManager : MonoBehaviour
{
    [SerializeField]
    GeneratorController[] generators;

    [SerializeField]
    RocketSpawner[] rocketLaunchers;

    [SerializeField]
    BossRadarController[] radars;

    PlayerController thePlayer;

    int numberOfGenerators;
    int generatorsCounter = 0;

    public int currentPhase = 0;


    public float angleToPlayer;


    // Start is called before the first frame update
    void Start()
    {
        numberOfGenerators = generators.Length;
        thePlayer = FindObjectOfType<PlayerController>();

        DeactivateRadars();
    }

    void DeactivateRadars()
    {
        for (int i = 0; i < radars.Length; i++)
        {
            GameObject radar = radars[i].gameObject;
            radar.SetActive(false);
        }
    }

    void ActivateRadarWithRotation(int numberOfRadarToActivate, float offsetInDeg)
    {
        for (int i = 0; i < radars.Length; i++)
        {
            GameObject radar = radars[i].gameObject;
            if (i == numberOfRadarToActivate)
            {
                radar.SetActive(true);
                //radar.transform.Rotate(new Vector3(0, 0, 1), 30, Space.Self);   
                radar.transform.Rotate(new Vector3(0, 0, 1), angleToPlayer - offsetInDeg, Space.Self);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 dir = (gameObject.transform.position - thePlayer.transform.position).normalized;
        angleToPlayer = Vector3.Angle(Vector3.up, dir);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            NextPhase();
        }
    }

    public void MinusGenerator()
    {
        NextPhase();
    }

    public void NextPhase()
    {
        currentPhase += 1;

        switch (currentPhase)
        {
            case 1:
                StartPhase1();
                break;
            case 2:
                StartPhase2();
                break;
            case 3:
                StartPhase3();
                break;
            case 4:
                StartPhase4();
                break;
        }
    }

    public void StartPhase1()
    {
        LaunchNewRocket();
        //Debug.Log("Phase 1");

    }

    public void StartPhase2()
    {
        LaunchNewRocket();
        DeactivateRadars();
        ActivateRadarWithRotation(0, 120);
        //Debug.Log("Phase 2");
    }

    public void StartPhase3()
    {
        LaunchNewRocket();
        DeactivateRadars();
        ActivateRadarWithRotation(3, -120);
        ActivateRadarWithRotation(4, -120);
        //Debug.Log("Phase 3");
    }

    public void StartPhase4()
    {
        //LaunchNewRocket();
        DeactivateRadars();
        //ActivateRadarWithRotation(5, 120);
        //ActivateRadarWithRotation(6, 120);
        //ActivateRadarWithRotation(7, 120);
        //Debug.Log("You can plant a bomb");
    }

    public void LaunchNewRocket()
    {
        RocketSpawner rs = ClosestLauncher();
        rs.LaunchRocket();
    }

    RocketSpawner ClosestLauncher()
    {
        float dstToPlayer = 1000;
        RocketSpawner closestSpawner = null;

        for (int i = 0; i < rocketLaunchers.Length; i++)
        {
            float currentDst = Vector3.Distance(thePlayer.transform.position, rocketLaunchers[i].transform.position);
            if (currentDst < dstToPlayer)
            {
                dstToPlayer = currentDst;
                closestSpawner = rocketLaunchers[i];
            }
        }

        return closestSpawner;
    }

}
