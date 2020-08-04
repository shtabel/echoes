using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageTrigger : MonoBehaviour
{
    [SerializeField]
    public int messageId;  // id of the message to call

    ChatManager chatMng;

    void Start()
    {
        chatMng = FindObjectOfType<ChatManager>();
    }

    public void CallMessage()
    {
        chatMng.TypeMessage(messageId);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            CallMessage();
        }

        gameObject.SetActive(false);
    }
}
