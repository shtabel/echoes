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
    BossRotationController minesStParent;            // parent of static mines

    // EMITTERS: phase 3
    [SerializeField]
    BossRotationController emittersParent;          // parent of emitters

    // MOVING MINES: phase 5
    [SerializeField]
    GameObject minesMovParent;                      // parent of moving mines

    // ROCKET LAUNCHERS: phases 2, 4, 6
    [SerializeField]
    RocketSpawner[] rocketLaunchers;                // array of rocket launchers

    // BOSS'S RADARS (SEARCHERS): phases 1, ???
    [SerializeField]
    BossRadarController[] radars;                   // array of radars (boss's searchers)

    Collider bossCollider;

    PlayerController thePlayer;                     // the player 

    [SerializeField]
    GameObject[] arches;
    

    public int currentPhase = 0;    // current phase of the boss fight
    
    // TIMING
    [SerializeField]
    float phaseOneTime;     // how long to survive during phase 1
    [SerializeField]
    float phaseThreeTime;   // how long to survive during phase 3
    [SerializeField]
    float phaseFiveTime;    // how long to survive during phase 5
    [SerializeField]
    float phaseFiveMinesTime;    // how long to survive during phase 5 (only mines)
    [SerializeField]
    float gapTime;          // gap btw generator is destroyed and start of next phase

    // doors before the boss
    [SerializeField]
    SliderDoorController[] theDoorsBefore;

    // Start is called before the first frame update
    void Start()
    {
        numberOfGenerators = generators.Length;
        thePlayer = FindObjectOfType<PlayerController>();

        DeactivateRadars();

        bossCollider = transform.parent.GetComponent<BoxCollider>();

        SetArch(0);

        CheckLeftPuzzle();
        CheckRightPuzzle();

        CheckBossBattle();
        
    }
    void Update()
    {
        //Vector3 dir = (gameObject.transform.position - thePlayer.transform.position).normalized;
        //angleToPlayer = Vector3.Angle(Vector3.up, dir);

        //if (Input.GetKeyDown(KeyCode.Space))    // switch to the 1st phase of the boss
        //{
        //    StartPhase1();
        //}

        //if (Input.GetKeyDown(KeyCode.B))    // launch a rocket
        //{
        //    LaunchNewRocket();
        //}


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
        minesStParent.gameObject.SetActive(true);
        minesStParent.SetRotationSpeed(0);

        // вычисляем угол до игрока
        Vector3 dirToPlayer = minesStParent.transform.position - thePlayer.transform.position;
        float angle = Vector3.Angle(Vector3.up, dirToPlayer);
        angle = (thePlayer.transform.position.x >= transform.position.x) ? angle : -angle;
        //Debug.Log("angle = " + angle);
        
        minesStParent.transform.Rotate(new Vector3(0, 0, 1), angle, Space.Self);
    }

    void ActivateEmitters(float speed)
    {
        emittersParent.gameObject.SetActive(true);
        emittersParent.SetRotationSpeed(speed);

        // вычисляем угол до игрока
        Vector3 dirToPlayer = emittersParent.transform.position - thePlayer.transform.position;
        float angle = Vector3.Angle(Vector3.up, dirToPlayer);
        angle = (thePlayer.transform.position.x >= transform.position.x) ? angle : -angle;
        //Debug.Log("angle = " + angle);
        
        emittersParent.transform.Rotate(new Vector3(0, 0, 1), angle + 20, Space.Self);        
    }

    void ActivateRadarWithRotation(int numberOfRadarToActivate, float offsetInDeg)
    {
        for (int i = 0; i < radars.Length; i++)
        {
            GameObject radar = radars[i].gameObject;
            if (i == numberOfRadarToActivate)       // активируем нужный радар
            {
                radar.SetActive(true);

                // вычисляем угол до игрока
                Vector3 dirToPlayer = radar.transform.position - thePlayer.transform.position;
                float angle = Vector3.Angle(Vector3.up, dirToPlayer);
                angle = (thePlayer.transform.position.x >= transform.position.x) ? angle : -angle;
                
                // задаем начальный поворот радара с учетом оффсета
                radar.transform.Rotate(new Vector3(0, 0, 1), angle + offsetInDeg, Space.Self);
            }
        }
    }        

    public void MinusGenerator()
    {
        GeneratorBlink();
        StartCoroutine(ExecuteNextPhaseAfterTime(gapTime));
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
            case 6:
                StartPhase6();
                break;
            case 7:
                FinishBattle();
                break;
        }
    }

    public void StartPhase1()
    {
        // просто убегаем от радара в течении заданного времени
        //Debug.Log("Phase 1");
        currentPhase = 1;
        SetArch(1);
        
        ActivateRadarWithRotation(0, -90);       

        StartCoroutine(ExecuteNextPhaseAfterTime(phaseOneTime));
    }   

    public void StartPhase2()
    {
        // надо уничтожить один из трех статичных генераторов за сче ракеты
        //Debug.Log("Phase 2");
        
        LaunchNewRocket();
        DeactivateRadars();
        ActivateGenerators(0);        
    }

    public void StartPhase3()
    {
        // надо убегать от эмиттеров и уклоняться от мин в течении заданного времени
        //Debug.Log("Phase 3");

        // deactivate previous objects
        generatorsParent.gameObject.SetActive(false);
        // activate new objects
        ActivateStaticMines();
        ActivateEmitters(20);

        /*  TO DO:
         *  надо сделать чтобы сначала появились мины
        *   потом не движующиеся эмиттеры, потом они начинают медленно перемещаться
        *   взатем они разгоняются до предела и начинается отчет выживания
        */

        StartCoroutine(ExecuteNextPhaseAfterTime(phaseThreeTime));

    }

    public void StartPhase4()
    {
        // неоьходимо уничтожить один из двух движущихся генераторов за счет ракеты
        //Debug.Log("Phase 4");

        // deactivate previous objects
        emittersParent.gameObject.SetActive(false);
        //minesStParent.gameObject.SetActive(false);
        minesStParent.gameObject.transform.Translate(new Vector3(-50, 0, 0));

        ActivateGenerators(10);
        LaunchNewRocket();
    }

    public void StartPhase5()
    {
        //Debug.Log("Phase 5");
        // deactivate previous objects
        generatorsParent.gameObject.SetActive(false);

        bossCollider.enabled = false;

        // activate new objects
        minesMovParent.SetActive(true);

        StartCoroutine(ActivateRadarAfterTime(phaseFiveMinesTime));

        StartCoroutine(ExecuteNextPhaseAfterTime(phaseFiveTime));
    }

    public void StartPhase6()
    {
        // неоьходимо уничтожить последний движущийся генератор за счет ракеты
        //Debug.Log("Phase 6");

        bossCollider.enabled = true;        

        // deactivate previous objects
        DeactivateRadars();
        minesMovParent.gameObject.SetActive(false);

        // activate new objects
        ActivateGenerators(-10);
        ActivateRadarWithRotation(3, -90);
        LaunchNewRocket();
    }

    void FinishBattle()
    {
        // deativate old objects
        DeactivateRadars();
        SetArch(2);

        FindObjectOfType<TimerManager>().StartTimer();

        FindObjectOfType<SaveManager>().SetBossBattleWon();
    }

    IEnumerator ActivateRadarAfterTime(float time)
    {
        yield return new WaitForSeconds(time);

        ActivateRadarWithRotation(5, -90);        
    }

    IEnumerator ExecuteNextPhaseAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        NextPhase();
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

    void SetArch(int number)
    {
        for (int i = 0; i < arches.Length; i++)
        {
            if (i == number)
            {
                arches[i].SetActive(true);
                MeshRenderer mr = arches[i].GetComponent<MeshRenderer>();
                mr.enabled = false;
            }
            else
            {
                arches[i].SetActive(false);
            }
        }
    }

    // not a battle but manager - door manager
    void CheckBossBattle()
    {
        // if the boss battle won
        if (FindObjectOfType<SaveManager>().CheckBossBattleWon())
        {
            FinishBattle();
        }
           
    }

    void CheckLeftPuzzle()
    {
        // if left puzzle is solved - handle doors
        if (FindObjectOfType<SaveManager>().CheckLeftPuzzleSolved())
        {
            // close the door to the puzzle
            theDoorsBefore[0].CloseTheDoor();
            // open the door to the boss
            theDoorsBefore[1].OpenTheDoor();
        }
    }

    void CheckRightPuzzle()
    {
        // if right puzzle is solved - handle doors
        if (FindObjectOfType<SaveManager>().CheckRightPuzzleSolved())
        {
            // close the door to the puzzle
            theDoorsBefore[2].CloseTheDoor();
            // open the door to the boss
            theDoorsBefore[3].OpenTheDoor();
        }
    }


}
