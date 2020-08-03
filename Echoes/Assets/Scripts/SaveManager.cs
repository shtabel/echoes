using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public bool startFromBeginning;         // true - start new game

    public Vector3 startPos;                // start position - at the beginning od the game

    public Vector3 lastChackpointPos;       // position of the last checkpoint

    public bool leftPuzzle;
    public bool rightPuzzle;

    public bool bossBattleWon;

    void Awake()
    {
        startFromBeginning = CheckStartFromBegining();

        if (startFromBeginning) //  new game
        {
            // to place the player in the beginning
            PlayerPrefs.SetFloat("lastCheckpointPosX", startPos.x);
            PlayerPrefs.SetFloat("lastCheckpointPosY", startPos.y);
            PlayerPrefs.SetFloat("lastCheckpointPosZ", startPos.z);

            // didn't solve boss's puzzles
            SetLeftPuzzleSolved(0);
            SetRightPuzzleSolved(0);

            // didn't won the boss battle
            SetBossBattleWon(0);

            // didn't complete the game
            SetStartFromBegining(0);
        }

        // place the player at he last checkpoint
        lastChackpointPos.x = PlayerPrefs.GetFloat("lastCheckpointPosX", startPos.x);
        lastChackpointPos.y = PlayerPrefs.GetFloat("lastCheckpointPosY", startPos.y);
        lastChackpointPos.z = PlayerPrefs.GetFloat("lastCheckpointPosZ", startPos.z);

        // check boss battle won 
        if (CheckBossBattleWon())
        {
            bossBattleWon = true;
            lastChackpointPos.x = 255.75f;
            lastChackpointPos.y = -207;
            lastChackpointPos.z = 0;
            //FindObjectOfType<PlayerController>().transform.Translate(new Vector3(255.75f, -207, 0));
        }
            

        // check puzzles 
        if (CheckLeftPuzzleSolved())
            leftPuzzle = true;
        if (CheckRightPuzzleSolved())
            rightPuzzle = true;
    }

    public void SetStartFromBegining(int fromBegining)
    {
        PlayerPrefs.SetInt("startFromBegining", fromBegining);
    }

    public bool CheckStartFromBegining()
    {
        int binarBool = PlayerPrefs.GetInt("startFromBegining");
        if (binarBool == 1)
            return true;
        else
            return false;
    }

    public void SetBossBattleWon(int isDefited)
    {
        PlayerPrefs.SetInt("bossDone", isDefited);
    }

    public bool CheckBossBattleWon()
    {
        int binarBool = PlayerPrefs.GetInt("bossDone");
        if (binarBool == 1)
            return true;
        else
            return false;
    }

    public void SetLeftPuzzleSolved(int isSolved)
    {
        PlayerPrefs.SetInt("bossLPuzzleSolved", isSolved);
    }

    public bool CheckLeftPuzzleSolved()
    {
        int binarBool = PlayerPrefs.GetInt("bossLPuzzleSolved");
        if (binarBool == 1)        
            return true;        
        else
            return false;        
    }

    public void SetRightPuzzleSolved(int isSolved)
    {
        PlayerPrefs.SetInt("bossRPuzzleSolved", isSolved);
    }

    public bool CheckRightPuzzleSolved()
    {
        int binarBool = PlayerPrefs.GetInt("bossRPuzzleSolved");
        if (binarBool == 1)
            return true;
        else
            return false;
    }
}
