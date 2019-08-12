using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    // PUBLIC INIT
    public GameObject pauseMenu;
    public GameObject startMenu;
    public GameObject endMenu;          // ссылка на end menu
    public GameObject deadMenu;         // ссылка на dead menu

    public Animator animator;

    public string nextLevel;

    public bool isPaused;

    // PRIVATE INIT
    string levelToLoad;
    PlayerController thePlayer;

    // Start is called before the first frame update
    void Start()
    {
        thePlayer = FindObjectOfType<PlayerController>();
        
        //startMenu.SetActive(true);
        //Time.timeScale = 0f;
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
        Time.timeScale = 1f;

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
            pauseMenu.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    public void StartGame()
    {
        isPaused = false;
        startMenu.SetActive(false);
        Time.timeScale = 1f;
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

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OpenPauseMenu();
        }
    }
}
