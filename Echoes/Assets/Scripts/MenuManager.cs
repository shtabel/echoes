using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    // PUBLIC INIT
    public GameObject pauseMenu;
    public GameObject deadMenu;         // ссылка на dead menu
    public GameObject infoMenu;
    public GameObject endMenu;
    public GameObject LoadingScreen;
    public GameObject SavingText;

    public Animator animator;

    [HideInInspector]
    public string curLevel;         // current level
    public string nextLevel;        // next level to load

    public bool infoDisplayed;      // показали ли информацию
    public float displayInfoDelay;  // задержка во времени

    [HideInInspector]
    public bool isPaused;

    // PRIVATE INIT
    [SerializeField]
    Vector3 startPos;

    string levelToLoad;
    PlayerController thePlayer;
    Vector3 playersVelocity;

    bool checkForRestart;       // когда игрок умер "пробел" - рестарт

    //EnemyController[] enemies;
    //Vector3[] enemiesVelocity;
    //RocketController[] rockets;
    //Vector3[] rocketsVelocity;


    // Start is called before the first frame update
    void Start()
    {
        curLevel = SceneManager.GetActiveScene().name;
        thePlayer = FindObjectOfType<PlayerController>();

        //enemies = FindObjectsOfType<EnemyController>();
        //enemiesVelocity = new Vector3[enemies.Length];

        //ReassignRockets();

        checkForRestart = false;

        if (curLevel == "lvl1")
        {
            DisplayInfoCor();
        }
    }
    
    public void ReassignRockets()
    {
        //rockets = FindObjectsOfType<RocketController>();
        //rocketsVelocity = new Vector3[rockets.Length];
    }

    public void DisplayInfoCor()
    {
        infoDisplayed = true;
        StartCoroutine(DisplayInfo());
    }

    IEnumerator DisplayInfo()
    {
        yield return new WaitForSeconds(displayInfoDelay);

        // показываем экран с информацией
        infoMenu.SetActive(true);        

        // ставим игру на паузу
        if (!isPaused)
        {
            isPaused = true;
            SetKinObjects(true);

            Time.timeScale = 0f;
        }
    }

    public void DisplaySaving(bool show)
    {
        SavingText.SetActive(show);

        if (show)
        {
            StartCoroutine(DeactivateSavingText(1));
        }

    }

    IEnumerator DeactivateSavingText(float time)
    {
        yield return new WaitForSeconds(time);

        DisplaySaving(false);
    }

    public void NewGame()
    {
        //PlayerPrefs.SetFloat("lastCheckpointPosX", startPos.x);
        //PlayerPrefs.SetFloat("lastCheckpointPosY", startPos.y);
        //PlayerPrefs.SetFloat("lastCheckpointPosZ", startPos.z);
        LoadingScreen.SetActive(true);

        levelToLoad = SceneManager.GetActiveScene().name;
        animator.SetTrigger("fadeOut");

        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        if (deadMenu.active == false)
        {
            ResumeGame();
        }
    }

    public void ResumeGame()
    {
        isPaused = false;
        pauseMenu.SetActive(false);
        infoMenu.SetActive(false);
        Time.timeScale = 1f;

        SetKinObjects(false);

        //thePlayer.MakeVisible(true);
    }

    public void MainMenu()
    {
        ResumeGame();
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void OpenPauseMenu()
    {
        if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            //thePlayer.MakeVisible(false);

            isPaused = true;
            SetKinObjects(true);

            pauseMenu.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    public void PlayerDead()
    {
        deadMenu.SetActive(true);
        checkForRestart = true;
        //Time.timeScale = 0f;
    }

    public void GameCompleted()
    {
        endMenu.SetActive(true);
        Time.timeScale = 0f;
        FindObjectOfType<SaveManager>().SetStartFromBegining(1);
        FindObjectOfType<SaveManager>().SetGamePlayed(0);
    }

    public void LevelCompleted()
    {
        levelToLoad = nextLevel;
        animator.SetTrigger("fadeOut"); 
        //SceneManager.LoadScene(nextLevel);
    }

    public void OnFadeCompleted()
    {
        SceneManager.LoadScene(levelToLoad);
    }

    void SetKinObjects(bool isKinematic)
    {
        if (isKinematic)
        {
            playersVelocity = thePlayer.rb.velocity;
            thePlayer.GetComponent<Rigidbody>().isKinematic = isKinematic;
        }
        else
        {
            thePlayer.GetComponent<Rigidbody>().isKinematic = isKinematic;
            thePlayer.rb.AddForce(playersVelocity, ForceMode.VelocityChange);
        }

        //if (rockets != null)
        //{
        //    if (isKinematic) // поставили на паузу
        //    {
        //        int counter = 0;
        //        foreach (EnemyController e in rockets)
        //        {
        //            // сохранить скорость
        //            rocketsVelocity[counter] = e.rb.velocity;
        //            counter++;

        //            // сделать кинематик
        //            e.SetKinematic(isKinematic);
        //        }
        //    }
        //    else
        //    {
        //        int counter = 0;
        //        foreach (EnemyController e in rockets)
        //        {
        //            // сделать НЕ кинематик
        //            e.SetKinematic(isKinematic);

        //            // добавить силу
        //            e.rb.AddForce(rocketsVelocity[counter], ForceMode.VelocityChange);
        //            counter++;
        //        }
        //    }

        //    //if (enemies != null)
        //    //{
        //    //    if (isKinematic) // поставили на паузу
        //    //    {
        //    //        int counter = 0;
        //    //        foreach (EnemyController e in enemies)
        //    //        {
        //    //            // сохранить скорость
        //    //            enemiesVelocity[counter] = e.rb.velocity;
        //    //            counter++;

        //    //            // сделать кинематик
        //    //            e.SetKinematic(isKinematic);
        //    //        }
        //    //    }
        //    //    else
        //    //    {
        //    //        int counter = 0;
        //    //        foreach (EnemyController e in enemies)
        //    //        {
        //    //            // сделать НЕ кинематик
        //    //            e.SetKinematic(isKinematic);

        //    //            // добавить силу
        //    //            e.rb.AddForce(enemiesVelocity[counter], ForceMode.VelocityChange);
        //    //            counter++;
        //    //        }
        //    //    }
        //}
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //ReassignRockets();
            if (!deadMenu.activeSelf)
            {
                OpenPauseMenu();
            }
            
        }
        if (deadMenu.activeSelf && Input.GetKeyDown(KeyCode.Space))
        {
            NewGame();   
        }
#if (UNITY_EDITOR)
        if (Input.GetKeyDown(KeyCode.I))
        {
            StartCoroutine(DisplayInfo());
        }
#endif
    }
}
