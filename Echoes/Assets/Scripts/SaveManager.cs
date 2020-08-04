using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public bool startFromBeginning;         // true - start new game

    public Vector3 startPos;                // start position - at the beginning od the game

    public Vector3 lastChackpointPos;       // position of the last checkpoint

    public bool leftPuzzle;                 // if the player solved puzzles before the boss
    public bool rightPuzzle;

    public int msgID;                       // id of the last message

    void Awake()
    {
        startFromBeginning = CheckStartFromBegining();

        if (startFromBeginning) //  new game
        {
            // to place the player in the beginning
            SetCheckpointPos(startPos);
            //PlayerPrefs.SetFloat("lastCheckpointPosX", startPos.x);
            //PlayerPrefs.SetFloat("lastCheckpointPosY", startPos.y);
            //PlayerPrefs.SetFloat("lastCheckpointPosZ", startPos.z);

            // didn't solve boss's puzzles
            SetLeftPuzzleSolved(0);
            SetRightPuzzleSolved(0);

            // didn't won the boss battle
            SetBossBattleWon(0);

            // didn't complete the game
            SetStartFromBegining(0);

            // didn't show any messages
            SetMessageID(0);            
        }

        // place the player at he last checkpoint
        lastChackpointPos.x = PlayerPrefs.GetFloat("lastCheckpointPosX", startPos.x);
        lastChackpointPos.y = PlayerPrefs.GetFloat("lastCheckpointPosY", startPos.y);
        lastChackpointPos.z = PlayerPrefs.GetFloat("lastCheckpointPosZ", startPos.z);

        // check boss battle won 
        if (CheckBossBattleWon())
        {
            // position the player above boss to start escaping
            lastChackpointPos.x = 255.75f;
            lastChackpointPos.y = -207;
            lastChackpointPos.z = 0;
        }
            

        // check puzzles 
        if (CheckLeftPuzzleSolved())
            leftPuzzle = true;
        if (CheckRightPuzzleSolved())
            rightPuzzle = true;
    }

    public void SetCheckpointPos(Vector3 pos)
    {
        PlayerPrefs.SetFloat("lastCheckpointPosX", pos.x);
        PlayerPrefs.SetFloat("lastCheckpointPosY", pos.y);
        PlayerPrefs.SetFloat("lastCheckpointPosZ", pos.z);
    }

    public int GetLastMessageID()
    {
        int id = PlayerPrefs.GetInt("messagesShowed");
        return id;
    }

    public void SetMessageID(int lastMessageID)
    {
        PlayerPrefs.SetInt("messagesShowed", lastMessageID);
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
