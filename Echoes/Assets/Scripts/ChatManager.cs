using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatManager : MonoBehaviour
{
    [SerializeField]
    Text chatText;

    int lastId;

    SaveManager saveManager;


    // Start is called before the first frame update
    void Start()
    {
        saveManager = FindObjectOfType<SaveManager>();

        chatText.supportRichText = true;

        DisplayOldMessages();
        
    }

    void DisplayOldMessages()
    {
        // get last message id
        lastId = saveManager.GetLastMessageID();

        // display all the messages from the 1st one to the last one
        if (lastId != 0)
        {
            for (int i = 1; i <= lastId; i++)
            {
                // display the message
                string msg = GetMessage(i);
                AddText(msg);
            }
        }
    }
    

    void AddText(string txt)
    {
        chatText.text += "\n......" + txt;
    }

    IEnumerator TypeText(string txt) // actually typing the text
    {
        chatText.text += "\n......";
        
        var charArray = txt.ToCharArray();

        foreach (char letter in txt.ToCharArray())
        {
            chatText.text += letter;

            yield return new WaitForSeconds(0.07f);
        }
    }

    public void TypeMessage(int id) // to start typing the message 
    {
        // если сообщение еще не напечатано
        if (lastId < id)
        {
            string text = GetMessage(id);            

            StartCoroutine(TypeText(text));

            lastId = id;
            //saveManager.SetMessageID(id);
        }       
    }
    

    string GetMessage(int id)
    {
        string msg = "";    // to store message 

        switch (id)         // get the message with id
        {
            case 1:
                msg = @"
Приветствую, пилот! Я - echOS, твой бортовой компьютер.
Я отвечаю за работу радара и других систем подводной лодки.";
                break;
            case 2:
                msg = @"
Этот желтый круг - радиомаячек. Активируй его для сохранения прогресса.";
                break;
            case 3:
                msg = @"
Внимание, опасность! Красные крестики обозначают мины. Держись от них подальше, иначе они потопят лодку.";
                break;
            case 4:
                msg = @"
Похоже, генератор питает дверь. Необходимо найти способ уничтожить его, чтобы пройти дальше.";
                break;
            case 5:
                msg = @"
Берегись! Это самонаводящаяся ракета, она ловит излуение радара и следует в место, где ты ее засек.";
                break;
            case 7:
                msg = @"
Эти радары сканируют местноть вокруг себя. Не попадайся на красный луч.";
                break;
            case 8:
                msg = @"
Похоже, необходимо включить определенные маяки, чтобы открыть дверь.";
                break;
            case 9:
                msg = @"
Ошибка 0x34243! Программный сбой .... отключение радара.
Мне нужно время на воостановление, придется тебе двигаться дальше вслепую.";
                break;
            case 10:
                msg = @"
Радар воостановлен";
                break;
            case 11:
                msg = @"
А вот и главный босс этой качалки. Уничтожь его!";
                break;
            case 12:
                msg = @"
Похоже, был запущен протокол самоликвидации.
Успей выбраться пока время не истекло!";
                break;
        }

        return msg;
    }
}
