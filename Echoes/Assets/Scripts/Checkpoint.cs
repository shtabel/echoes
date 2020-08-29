using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField]
    int lastMassageID;

    SaveManager sm;
    AudioManager am;

    void Start()
    {
        sm = FindObjectOfType<SaveManager>();
        am = FindObjectOfType<AudioManager>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (transform.position != sm.GetCheckpointPos())
            {
                am.Play("save");
                FindObjectOfType<MenuManager>().DisplaySaving(true);
            }            

            sm.SetCheckpointPos(transform.position);
            //PlayerPrefs.SetFloat("lastCheckpointPosX", transform.position.x);
            //PlayerPrefs.SetFloat("lastCheckpointPosY", transform.position.y);
            //PlayerPrefs.SetFloat("lastCheckpointPosZ", transform.position.z);

            if (lastMassageID != 0) 
            {
                sm.SetMessageID(lastMassageID);
            }

            
            
        }


    }
}
