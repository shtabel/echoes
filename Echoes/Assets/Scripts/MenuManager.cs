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

    public Animator animator;

    [HideInInspector]
    public string curLevel;         // current level
    public string nextLevel;        // next level to load

    public bool infoDisplayed;      // показали ли информацию
    public float displayInfoDelay;  // задержка во времени

    [HideInInspector]
    public bool isPaused;

    // PRIVATE INIT
    string levelToLoad;
    PlayerController thePlayer;

    EnemyController[] enemies;
    
    // Start is called before the first frame update
    void Start()
    {
        curLevel = SceneManager.GetActiveScene().name;
        thePlayer = FindObjectOfType<PlayerController>();

        enemies = FindObjectsOfType<EnemyController>();

        if (curLevel == "lvl1")
        {
            DisplayInfoCor();
        }
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

    public void NewGame()
    {
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

        thePlayer.MakeVisible(true);
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
            thePlayer.MakeVisible(false);

            isPaused = true;
            SetKinObjects(true);

            pauseMenu.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    public void PlayerDead()
    {
        deadMenu.SetActive(true);
        //Time.timeScale = 0f;
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
        thePlayer.GetComponent<Rigidbody>().isKinematic = isKinematic;

        if (enemies != null)
        {
            foreach (EnemyController e in enemies)
            {
                e.SetKinematic(isKinematic);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OpenPauseMenu();
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            StartCoroutine(DisplayInfo());
        }
    }
}
