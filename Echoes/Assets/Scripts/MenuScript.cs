using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    [SerializeField]
    GameObject continueButton;

    [SerializeField]
    GameObject loadingText;

    SaveManager sm;

    void Start()
    {
        sm = FindObjectOfType<SaveManager>();

        if (!sm.CheckGamePlayed())
        {
            continueButton.SetActive(false);
        }
    }

    public void StartGame() // start new game
    {
        loadingText.SetActive(true);
        sm.SetStartFromBegining(1);
        sm.SetGamePlayed(1);
        SceneManager.LoadScene(1);
    }

    public void ContinueGame()
    {
        loadingText.SetActive(true);
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Debug.Log("Quit game");
        Application.Quit();
    }
}
