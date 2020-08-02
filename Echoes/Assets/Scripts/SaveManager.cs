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
        if (startFromBeginning) //  new game
        {
            // to place the player in the beginning
            PlayerPrefs.SetFloat("lastCheckpointPosX", startPos.x);
            PlayerPrefs.SetFloat("lastCheckpointPosY", startPos.y);
            PlayerPrefs.SetFloat("lastCheckpointPosZ", startPos.z);

            // didn't solve boss's puzzles
            PlayerPrefs.SetInt("bossLPuzzleSolved", 0);
            PlayerPrefs.SetInt("bossRPuzzleSolved", 0);

            // didn't won the boss battle
            PlayerPrefs.SetInt("bossDone", 0);
        }

        // place the player at he last checkpoint
        lastChackpointPos.x = PlayerPrefs.GetFloat("lastCheckpointPosX", startPos.x);
        lastChackpointPos.y = PlayerPrefs.GetFloat("lastCheckpointPosY", startPos.y);
        lastChackpointPos.z = PlayerPrefs.GetFloat("lastCheckpointPosZ", startPos.z);

        // check boss battle won 
        if (CheckBossBattleWon())
        {
            bossBattleWon = true;
            FindObjectOfType<PlayerController>().transform.Translate(new Vector3(255.75f, -207, 0));
        }
            

        // check puzzles 
        if (CheckLeftPuzzleSolved())
            leftPuzzle = true;
        if (CheckRightPuzzleSolved())
            rightPuzzle = true;
    }


    public void SetBossBattleWon()
    {
        PlayerPrefs.SetInt("bossDone", 1);
    }

    public bool CheckBossBattleWon()
    {
        int binarBool = PlayerPrefs.GetInt("bossDone");
        if (binarBool == 1)
            return true;
        else
            return false;
    }

    public void SetLeftPuzzleSolved()
    {
        PlayerPrefs.SetInt("bossLPuzzleSolved", 1);
    }

    public bool CheckLeftPuzzleSolved()
    {
        int binarBool = PlayerPrefs.GetInt("bossLPuzzleSolved");
        if (binarBool == 1)        
            return true;        
        else
            return false;        
    }

    public void SetRightPuzzleSolved()
    {
        PlayerPrefs.SetInt("bossRPuzzleSolved", 1);
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
