using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerPos : MonoBehaviour
{
    SaveManager sm;

    // Start is called before the first frame update
    void Start()
    {
        sm = FindObjectOfType<SaveManager>();
        transform.position = sm.lastChackpointPos;
    }

//    void Update()
//    {
//#if (UNITY_EDITOR)
//        if (Input.GetKeyDown(KeyCode.Space))
//        {
//            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
//        }
//#endif
//    }
}
