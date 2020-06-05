using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatManager : MonoBehaviour
{
    [SerializeField]
    Text chatText;

    bool sawMine;

    bool startMarkup;
    int closedBracket;

    // Start is called before the first frame update
    void Start()
    {
        chatText.supportRichText = true;

    }

    // Update is called once per frame
    void Update()
    {

    }

    void AddText(string txt)
    {
        chatText.text += "\n" + txt;
    }

    IEnumerator TypeText(string txt)
    {
        chatText.text += "\n";

        //string tempText = "";    // text to store <>...</>

        var charArray = txt.ToCharArray();

        foreach (char letter in txt.ToCharArray())
        {
            //if (letter == '<')
            //{
            //    startMarkup = true;
            //}            
            //if (letter == '>')
            //{
            //    closedBracket++;
            //    if (closedBracket == 2)
            //    {
            //        startMarkup = false;
            //        closedBracket = 0;
            //        chatText.text += tempText;
            //    }
            //}

            //if (startMarkup)
            //{
            //    tempText += letter.ToString();
            //}
            //else
            //{
            //    chatText.text += letter;
            //}

            chatText.text += letter;

            yield return new WaitForSeconds(0.1f);
        }
    }

    public void DisplayMessage(string message_name)
    {
        switch (message_name)
        {
            case "intro":
                AddText(@"Hello, this is Dr. Hui.
You was chosen to be a pilot of this submarine.");
                break;
            case "mine":
                if (!sawMine)
                {
                    //                    AddText(@"Be carefull these <color=red>red crosses</color> are <color=red>mines</color>.
                    //You touch them - you dead!");
                    StartCoroutine(TypeText(@"Be carefull these <color=red>red crosses</color> are <color=red>mines</color>.
You touch them - you dead!"));
                    sawMine = true;
                }                
                break;
        }
    }
}
