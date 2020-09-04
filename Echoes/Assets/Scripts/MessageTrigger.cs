using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageTrigger : MonoBehaviour
{
    [SerializeField]
    public int messageId;  // id of the message to call

    ChatManager chatMng;

    AudioManager am;

    void Start()
    {
        am = FindObjectOfType<AudioManager>();
        chatMng = FindObjectOfType<ChatManager>();
    }

    public void CallMessage()
    {
        am.Play("msg_show");
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
