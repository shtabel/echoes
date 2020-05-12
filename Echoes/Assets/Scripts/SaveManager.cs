using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    [SerializeField]
    public Vector3 startPos;

    public Vector3 lastChackpointPos;

    void Awake()
    {
        lastChackpointPos.x = PlayerPrefs.GetFloat("lastCheckpointPosX", startPos.x);
        lastChackpointPos.y = PlayerPrefs.GetFloat("lastCheckpointPosY", startPos.y);
        lastChackpointPos.z = PlayerPrefs.GetFloat("lastCheckpointPosZ", startPos.z);
    }
}
