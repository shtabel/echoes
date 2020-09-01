using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerManager : MonoBehaviour
{
    [SerializeField]
    Text timerText; 
    [SerializeField]
    float initTime; // inital count down time
    [SerializeField]
    float speedUp;

    float timer;    // remaining time
    bool canCount;
    bool doOnce;

    // in the format mm:ss
    float minutes;  
    float seconds;

    string mm;
    string ss;

    PlayerController thePlayer;

    // Start is called before the first frame update
    void Start()
    {
        timer = initTime;
        //canCount = true;

        thePlayer = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (timer <= 0.0f && !doOnce)
        {
            GameOver();
        }
        else if (timer >= 0.0f && canCount)
        {
            DisplayTime();
        }

        if (timer < 10)
        {
            speedUp = 0.7f;
        }
    }

    public void StartTimer()
    {
        canCount = true;
        timerText.gameObject.SetActive(true);
    }

    public void StopTimer()
    {
        canCount = false;
        //timerText.gameObject.SetActive(true);
    }

    void GameOver()
    {
        doOnce = true;

        canCount = false;
        timerText.text = "00:00";
        timer = 0.0f;

        thePlayer.DestroyPlayer();
    }

    void DisplayTime()
    {


        timer -= Time.deltaTime * speedUp;

        // get time in format mm:ss
        minutes = Mathf.Floor(timer / 60);
        seconds = Mathf.RoundToInt(timer % 60);

        mm = minutes.ToString();
        ss = seconds.ToString();

        if (minutes < 10)
        {
            mm = "0" + minutes.ToString();
        }
        if (seconds < 10)
        {
            ss = "0" + Mathf.RoundToInt(seconds).ToString();
        }

        timerText.text = mm + ":" + ss;
    }
}
