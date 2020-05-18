using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public bool startFromBeginning;

    public Vector3 startPos;

    public Vector3 lastChackpointPos;

    void Awake()
    {
        if (startFromBeginning)
        {
            PlayerPrefs.SetFloat("lastCheckpointPosX", startPos.x);
            PlayerPrefs.SetFloat("lastCheckpointPosY", startPos.y);
            PlayerPrefs.SetFloat("lastCheckpointPosZ", startPos.z);
        }
        lastChackpointPos.x = PlayerPrefs.GetFloat("lastCheckpointPosX", startPos.x);
        lastChackpointPos.y = PlayerPrefs.GetFloat("lastCheckpointPosY", startPos.y);
        lastChackpointPos.z = PlayerPrefs.GetFloat("lastCheckpointPosZ", startPos.z);
    }
}
