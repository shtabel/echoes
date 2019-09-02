using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndMarkScript : MonoBehaviour
{
    [SerializeField]
    private float minVisibleDst;

    private GameObject endMarker;
    private GameObject endObject;

    private Rigidbody rb;

    private bool playerDead;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        endObject = GameObject.FindGameObjectWithTag("end");
        
        endMarker = this.transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        // rotate towards end object
        Vector3 dirToEnd =  endObject.transform.position - transform.position;
        float angle = Mathf.Atan2(dirToEnd.y, dirToEnd.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, angle);

        // check distance
        float dst = Vector3.Magnitude(dirToEnd);
        //Debug.Log(dst);
        if (dst <= minVisibleDst && endMarker.activeSelf)
        {
            SetMarker(false);
        }
        else if (dst > minVisibleDst && !endMarker.activeSelf && !playerDead)
        {
            SetMarker(true);
        }
    }

    void SetMarker(bool isActive)
    {
        endMarker.SetActive(isActive);
    }

    public void SetMarker(bool isActive, bool playerIsDead)
    {
        endMarker.SetActive(isActive);
        playerDead = playerIsDead;
    }
}
