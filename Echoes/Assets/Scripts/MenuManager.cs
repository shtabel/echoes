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
    [SerializeField]
    Vector3 startPos;

    string levelToLoad;
    PlayerController thePlayer;
    Vector3 playersVelocity;

    EnemyController[] enemies;
    Vector3[] enemiesVelocity;
    

    // Start is called before the first frame update
    void Start()
    {
        curLevel = SceneManager.GetActiveScene().name;
        thePlayer = FindObjectOfType<PlayerController>();

        enemies = FindObjectsOfType<EnemyController>();
        enemiesVelocity = new Vector3[enemies.Length];

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
        //PlayerPrefs.SetFloat("lastCheckpointPosX", startPos.x);
        //PlayerPrefs.SetFloat("lastCheckpointPosY", startPos.y);
        //PlayerPrefs.SetFloat("lastCheckpointPosZ", startPos.z);


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
        

        if (enemies != null)
        {
            if (isKinematic) // поставили на паузу
            {
                int counter = 0;
                foreach (EnemyController e in enemies)
                {
                    // сохранить скорость
                    enemiesVelocity[counter] = e.rb.velocity;
                    counter++;

                    // сделать кинематик
                    e.SetKinematic(isKinematic);
                }
            }
            else
            {
                int counter = 0;
                foreach (EnemyController e in enemies)
                {
                    // сделать НЕ кинематик
                    e.SetKinematic(isKinematic);

                    // добавить силу
                    e.rb.AddForce(enemiesVelocity[counter], ForceMode.VelocityChange);
                    counter++;
                }
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
