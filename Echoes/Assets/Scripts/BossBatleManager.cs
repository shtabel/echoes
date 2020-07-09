using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBatleManager : MonoBehaviour
{
    // GENERATORS: phases 2, 4, 6
    [SerializeField]
    BossRotationController generatorsParent;    // parent of generators that we rotate
    [SerializeField]
    GeneratorController[] generators;           // array of generators
    int numberOfGenerators;                     // nuber of generators (3)
    int generatorsCounter = 0;                  // how much generators are destroyed

    // STATIC MINES: phase 3
    [SerializeField]
    BossRotationController minessParent;            // parent of  static mines

    // EMITTERS: phase 3
    [SerializeField]
    BossRotationController emittersParent;          // parent of emitters

    // ROCKET LAUNCHERS: phases 2, 4, 6
    [SerializeField]
    RocketSpawner[] rocketLaunchers;                // array of rocket launchers

    // BOSS'S RADARS (SEARCHERS): phases 1, ???
    [SerializeField]
    BossRadarController[] radars;                   // array of radars (boss's searchers)

    

    PlayerController thePlayer;                     // the player 

    

    public int currentPhase = 0;    // current phase of the boss fight
    
    // TIMING
    [SerializeField]
    float phaseOneTime;     // how long to survive during phase 1
    [SerializeField]
    float phaseThreeTime;     // how long to survive during phase 3
    [SerializeField]
    float gapTime;          // gap btw generator is destroyed and start of next phase

    // Start is called before the first frame update
    void Start()
    {
        numberOfGenerators = generators.Length;
        thePlayer = FindObjectOfType<PlayerController>();

        DeactivateRadars();
    }
    void Update()
    {
        Vector3 dir = (gameObject.transform.position - thePlayer.transform.position).normalized;
        //angleToPlayer = Vector3.Angle(Vector3.up, dir);

        if (Input.GetKeyDown(KeyCode.Space))    // switch to the next phase of the boss
        {
            NextPhase();
        }

        if (Input.GetKeyDown(KeyCode.B))    // launch a rocket
        {
            LaunchNewRocket();
        }


        //// check emitters angle to the playre
        //Vector3 dirToPlayer = emittersParent.transform.position - thePlayer.transform.position;
        //Debug.Log("dirToPlayer = " + dirToPlayer);
        //float angle = Vector3.Angle(Vector3.up, dirToPlayer);
        //Debug.Log("angle = " + angle);
    }


    void DeactivateRadars()
    {
        for (int i = 0; i < radars.Length; i++)
        {
            GameObject radar = radars[i].gameObject;
            radar.SetActive(false);
        }
    }

    void ActivateGenerators(float speed)
    {
        generatorsParent.gameObject.SetActive(true);
        generatorsParent.SetRotationSpeed(speed);
    }

    void ActivateStaticMines()
    {
        minessParent.gameObject.SetActive(true);
        minessParent.SetRotationSpeed(0);

        Vector3 dirToPlayer = minessParent.transform.position - thePlayer.transform.position;

        float angle = Vector3.Angle(Vector3.up, dirToPlayer);
        Debug.Log("angle = " + angle);

        //emittersParent.transform.LookAt(thePlayer.transform);
        minessParent.transform.Rotate(new Vector3(0, 0, 1), angle, Space.Self);
    }

    void ActivateEmitters(float speed)
    {
        emittersParent.gameObject.SetActive(true);
        emittersParent.SetRotationSpeed(speed);

        Vector3 dirToPlayer = emittersParent.transform.position - thePlayer.transform.position;

        float angle = Vector3.Angle(Vector3.up, dirToPlayer);
        Debug.Log("angle = " + angle);

        //emittersParent.transform.LookAt(thePlayer.transform);
        emittersParent.transform.Rotate(new Vector3(0, 0, 1), angle + 20, Space.Self);        
    }

    void ActivateRadarWithRotation(int numberOfRadarToActivate, float offsetInDeg)
    {
        for (int i = 0; i < radars.Length; i++)
        {
            GameObject radar = radars[i].gameObject;
            if (i == numberOfRadarToActivate)
            {
                radar.SetActive(true);

                if (thePlayer.transform.position.x == transform.position.x)
                {
                    // так как неправльно вращается
                    radar.transform.LookAt(new Vector3
                        (thePlayer.transform.position.x+1, thePlayer.transform.position.y, thePlayer.transform.position.z));
                } else
                    radar.transform.LookAt(thePlayer.transform);

                //radar.transform.Rotate(new Vector3(0, 0, 1), 30, Space.Self);   
                radar.transform.Rotate(new Vector3(0, 1, 0), offsetInDeg, Space.Self);
            }
        }
    }        

    public void MinusGenerator()
    {
        GeneratorBlink();
        StartCoroutine(ExecuteNextPhaseAfterTime(gapTime));
        //NextPhase();
    }

    void GeneratorBlink()
    {
        bool generatorDestroyed = false;

        for (int i = 0; i < generators.Length; i++)
        {
            if (generators[i] == null)
                generatorDestroyed = true;
        }

        if (!generatorDestroyed)
        {
            for (int i = 0; i < generators.Length; i++)
            {
                if (generators[i].GetComponent<GeneratorController>())
                {
                    generators[i].GetComponent<GeneratorController>().Fade(false);
                }
            }
        }
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
            case 5:
                StartPhase5();
                break;
        }
    }

    public void StartPhase1()
    {
        //LaunchNewRocket();
        Debug.Log("Phase 1");
        currentPhase = 1;

        ActivateRadarWithRotation(0, -90);

        StartCoroutine(ExecuteNextPhaseAfterTime(phaseOneTime));
    }

    IEnumerator ExecuteNextPhaseAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        NextPhase();
    }

    public void StartPhase2()
    {
        Debug.Log("Phase 2");
        LaunchNewRocket();
        DeactivateRadars();
        ActivateGenerators(0);        
    }

    public void StartPhase3()
    {
        Debug.Log("Phase 3");
        // deactivate previous objects
        generatorsParent.gameObject.SetActive(false);
        // activate new objects
        ActivateStaticMines();
        ActivateEmitters(20);

        StartCoroutine(ExecuteNextPhaseAfterTime(phaseThreeTime));

    }

    public void StartPhase4()
    {
        Debug.Log("Phase 4");
        // deactivate previous objects
        emittersParent.gameObject.SetActive(false);
        minessParent.gameObject.SetActive(false);

        ActivateGenerators(10);
        LaunchNewRocket();
    }

    public void StartPhase5()
    {
        Debug.Log("Phase 5");
        // deactivate previous objects
        generatorsParent.gameObject.SetActive(false);
        

        //StartCoroutine(ExecuteNextPhaseAfterTime(phaseThreeTime));

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
