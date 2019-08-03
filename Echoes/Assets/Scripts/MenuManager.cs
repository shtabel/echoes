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

    public bool isPaused;

    // Start is called before the first frame update
    void Start()
    {
        //startMenu.SetActive(true);
        //Time.timeScale = 0f;
    }
    public void NewGame()
    {
        ResumeGame();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ResumeGame()
    {
        isPaused = false;
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
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
        Debug.Log("Level completed!");

        endMenu.SetActive(true);
        Time.timeScale = 0f;
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
